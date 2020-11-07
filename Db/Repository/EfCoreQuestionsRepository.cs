using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public List<Question> GetQuestions(string tag)
        {
            var qResult = new List<Question>();
            var quests = _context.Questions.AsQueryable();
            foreach (var q in quests)
            {
                if (q.tags.Contains(tag))
                    qResult.Add(q);
            }

            return qResult;

            //  var qss = _context.Questions.Where(question => question.tags.Contains(tag)).Include(question => question.tags);//var questions = _context.Questions.Where(question => question.tags.Contains(tag)==true).AsQueryable();
            //return questions.AsAsyncEnumerable();

            //  return  qss.AsQueryable().AsAsyncEnumerable();

        }

        // asp.net-core
    }
}
