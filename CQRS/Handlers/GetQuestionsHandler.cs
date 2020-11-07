using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using QuestionServiceWebApi.CQRS.Queries;
using QuestionServiceWebApi.Db.Repository;
using QuestionServiceWebApi.Models;

namespace QuestionServiceWebApi.CQRS.Handlers
{
    public class GetQuestionsHandler: IRequestHandler<GetQuestionsQuery, List<Question>>
    {
        private readonly EfCoreQuestionsRepository _questionsRepository;

        public GetQuestionsHandler(EfCoreQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }
        public Task<List<Question>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = _questionsRepository.GetQuestions(request.Tag);
           // return questions;
            return Task.FromResult(questions);
        }

        
    }
}
