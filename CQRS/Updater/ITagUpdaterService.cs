namespace QuestionServiceWebApi.CQRS.Updater
{
    public interface ITagUpdaterService
    {
        void Start();
        void StopInternal();
    }
}