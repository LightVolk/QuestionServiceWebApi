using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using QuestionServiceWebApi.Controllers.Infrastructure;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.CQRS.Updater.QUpdater
{
    public class QuestionUpdaterService : IHostedService, IQuestionUpdaterService
    {
        private int _updateTime;
        private DbContextOptions<ApplicationContext> _options;
        private Timer _timer;
        private IQuestionService _questionService;
        public QuestionUpdaterService(int updateTimeMilliseconds, DbContextOptions<ApplicationContext> options, IQuestionService questionService)
        {
            _updateTime = updateTimeMilliseconds;
            _options = options;
            _questionService = questionService;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(StartInternal, null, 0, _updateTime);
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await StopInternal();
        }

        private void StartInternal(object state)
        {
            Log.Write(LogEventLevel.Information, "Timed Background Service Tags Updater is starting.");

            _timer?.Change(Timeout.Infinite, 0);
            using var context = new ApplicationContext(_options);

            var qs = new HashSet<Question>();
            var tags = context.Tags.ToList();
            foreach (var tag in tags)
            {
                var searchResult = _questionService.GetQuestionsAsync(tag.Name).Result;
                if(searchResult.items!=null)
                    qs.UnionWith(searchResult.items);                
            }

            var titles = qs.Select(x => x.title);
            var updatedQuestions = context.Questions.Where(x =>titles.Equals(x.title)).AsQueryable().ToList();
            var insertedQuestions = new HashSet<Question>();

            foreach(var t in titles)
            {
                var q = context.Questions.FirstOrDefault(x => x.title.Equals(t));
                if (q!=null)
                {
                    insertedQuestions.Add(q);
                }
            }

            foreach(var q in context.Questions)
            {               
                foreach(var t in titles)
                {
                    if (q.title.Equals(t))
                    {
                        updatedQuestions.Add(q);                       
                    }
                }             
            }
            

            context.Questions.AddRange(insertedQuestions);
            context.Questions.UpdateRange(updatedQuestions);
            context.SaveChanges();


            _timer?.Change(5 * 1000 * 60, 0);
        }

        private Task StopInternal()
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Timed Background Service Question Updater is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
