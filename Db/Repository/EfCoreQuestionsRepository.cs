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
                if (q.Tags.Contains(tag))
                    qResult.Add(q);
            }

            //return qResult;
            //  var qss = _context.Questions.Where(question => question.Tags.Contains(tag)).Include(question => question.Tags);
            //var questions = _context.Questions.GroupBy(question => question.Tags.Contains("test")).AsQueryable().ToList();
            //var tags = _context.Questions.Include((x => x.Tags)).AsQueryable();

            //var questions = _context.Questions.Where(q => q.Tags.Contains("asp.net-core")).AsQueryable();
            return qResult;
        }

        // asp.net-core
    }

    public class Module
    {
        public List<Module> Modules=new List<Module>();
    }
}
