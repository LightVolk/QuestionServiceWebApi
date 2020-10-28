using QuestionServiceWebApi.Controllers.Infrastructure;
using QuestionServiceWebApi.Models;
using System.Threading.Tasks;

namespace QuestionServiceWebApi
{
    internal class App
    {
        public async Task<Search> RunAsync(QuestionService service)
        {
            var result= await service.GetQuestionsAsync("java");
            return result;
        }
    }
}