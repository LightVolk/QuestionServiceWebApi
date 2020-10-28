using System.Threading.Tasks;

namespace QuestionServiceWebApi.Controllers.Infrastructure
{
    public interface IQuestionService
    {
        Task GetQuestionsAsync();
    }
}