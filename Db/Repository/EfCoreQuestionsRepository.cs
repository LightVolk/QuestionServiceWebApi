using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuestionServiceWebApi.Models;

namespace QuestionServiceWebApi.Db.Repository
{
    public class EfCoreQuestionsRepository:EfCoreRepository<Question,ApplicationContext>
    {
        private readonly ApplicationContext _context;
        public EfCoreQuestionsRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public IAsyncEnumerable<Question> GetQuestions(string tag)
        {
            var questions =  _context.Questions.Where(question => question.tags.Contains(tag)).AsQueryable();
            return questions.AsAsyncEnumerable();
        }
}
}
