using Microsoft.Extensions.Hosting;
using QuestionServiceWebApi.Db;
using QuestionServiceWebApi.Db.Repository;
using QuestionServiceWebApi.Infrastructure;
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
        //private EfCoreTagRepository _tagRepository;
        public TagUpdaterService(ulong updateTimeMilliseconds, IEnumerable<string> tags/*, EfCoreTagRepository efCoreTagRepository*/)
        {
            _updateTime = updateTimeMilliseconds;
            _tags = tags;
            //_tagRepository = efCoreTagRepository;
        }

        

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await StartInternal();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await StopInternal();
        }

        private Task StartInternal()
        {
            var tcs = new TaskCompletionSource(); // сделаем метод асинхронным искусственно
            
            using var context = new ApplicationContext();

            //var tags = _tagRepository.GetAll();
            foreach (var tag in _tags)
            {                
                if (!context.Tags.Any(x => x.Name == tag))
                {
                    context.Tags.AddAsync(new Models.Tag() { Name = tag });
                }
            }

            tcs.SetResult();
            return tcs.Task;
        }
        private Task StopInternal()
        {
            return Task.CompletedTask;
        }

      
    }
}
