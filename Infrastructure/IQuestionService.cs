using QuestionServiceWebApi.Models;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Controllers.Infrastructure
{
    public interface IQuestionService
    {
        Task<Search> GetQuestionsAsync(string tag);
    }
}