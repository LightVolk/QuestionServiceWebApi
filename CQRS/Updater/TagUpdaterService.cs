using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Db.Repository;
using QuestionServiceWebApi.Infrastructure;
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
        private ulong _updateTime;
        private IEnumerable<string> _tags;
        private Timer _timer;
        private DbContextOptions<ApplicationContext> _options;
        //private EfCoreTagRepository _tagRepository;
        public TagUpdaterService(ulong updateTimeMilliseconds, IEnumerable<string> tags/*, EfCoreTagRepository efCoreTagRepository*/, DbContextOptions<ApplicationContext> options)
        {
            _updateTime = updateTimeMilliseconds;
            _tags = tags;
            _options = options;
            //_tagRepository = efCoreTagRepository;
        }

        

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(StartInternal, null, TimeSpan.Zero,
            TimeSpan.FromMinutes(5));
            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await StopInternal();
        }

        private void StartInternal(object state)
        {
            Log.Write(LogEventLevel.Information, "Timed Background Service Tags Updater is starting.");
            using var context = new ApplicationContext(_options);
           
            foreach (var tag in _tags)
            {                
                if (!context.Tags.Any(x => x.Name == tag))
                {
                    context.Tags.AddAsync(new Models.Tag() { Name = tag });
                }
            }

            context.SaveChanges();          
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
