﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using QuestionServiceWebApi.Controllers.Infrastructure;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Db.Repository;
using QuestionServiceWebApi.Infrastructure;
using QuestionServiceWebApi.Models;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.CQRS.Updater
{
    /// <summary>
    /// Сервис обновления тегов
    /// </summary>
    public class TagUpdaterService : IHostedService
    {
        private int _updateTime;
        private IEnumerable<string> _tags;
        private Timer _timer;
        private DbContextOptions<ApplicationContext> _options;
       
        //private EfCoreTagRepository _tagRepository;
        public TagUpdaterService(int updateTimeMilliseconds, IEnumerable<string> tags/*, EfCoreTagRepository efCoreTagRepository*/, DbContextOptions<ApplicationContext> options)
        {
            _updateTime = updateTimeMilliseconds;
            _tags = tags;
            _options = options;
           
            //_tagRepository = efCoreTagRepository;
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

            foreach (var tag in _tags)
            {
                if (!context.Tags.Any(x => x.Name == tag))
                {
                    context.Tags.AddAsync(new Models.Tag() { Name = tag });
                }
            }

            context.SaveChanges();




            //var updates = new List<Question>();

            //Dictionary<string, List<Question>> tagToQuestions = new Dictionary<string, List<Question>>();
            //foreach (var tag in context.Tags)
            //{
            //    var searchResult = _questionService.GetQuestionsAsync(tag.Name).Result;
            //    if (searchResult.items != null && searchResult.items.Any())
            //    {
            //        updates.AddRange(searchResult.items);

            //        if (!tagToQuestions.ContainsKey(tag.Name))
            //            tagToQuestions.Add(tag.Name, updates);
            //        else
            //            tagToQuestions[tag.Name].AddRange(updates);
            //    }
            //}

            //foreach (var tag in tagToQuestions.Keys)
            //{
            //    //context.Questions.AttachRange(tagToQuestions[tag]);
            //    var questions = tagToQuestions[tag];
            //    foreach(var q in questions)
            //    {
            //        if(context.Questions.Contains(q))
            //    }
            //    context.Questions.UpdateRange(tagToQuestions[tag]);
            //    context.SaveChanges();

            //}



            _timer?.Change(5 * 1000 * 60, 0);
        }
        private Task StopInternal()
        {
            Log.Write(Serilog.Events.LogEventLevel.Information, "Timed Background Service Tags Updater is stopping.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
