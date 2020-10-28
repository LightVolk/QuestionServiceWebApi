using QuestionServiceWebApi.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace QuestionServiceWebApi.Controllers.Infrastructure
{
    /// <summary>
    /// Сервис вопросов по SO
    /// </summary>
    public class QuestionService
    {
       
        private RestClient _restclient;
        /// <summary>
        /// конструктор
        /// </summary>     
        public QuestionService()
        {           
            _restclient= new RestClient("http://api.stackexchange.com/2.2/");
        }

        /// <summary>
        /// Получить список тем
        /// <paramref name="tag">тег, по которому можно получить ответы</paramref>
        /// </summary>
        /// <returns>объект-хранилище со списком тем</returns>
        public async Task<Search> GetQuestionsAsync(string tag)
        {            
            var request = new RestRequest($"search/advanced?order=desc&sort=activity&accepted=False&tagged={tag}&site=stackoverflow", Method.GET);         
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = _restclient.Execute<Search>(request);

            var tcs = new TaskCompletionSource<Search>(); // сделаем метод асинхронным искусственно
            tcs.SetResult(queryResult.Data);
            return await tcs.Task;
        }
    }
}