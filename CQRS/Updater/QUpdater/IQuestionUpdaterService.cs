using System.Threading;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.CQRS.Updater.QUpdater
{
    public interface IQuestionUpdaterService
    {
        void Dispose();
        Task StartAsync(CancellationToken cancellationToken);
        Task StopAsync(CancellationToken cancellationToken);
    }
}