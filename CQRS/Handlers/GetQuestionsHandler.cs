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
    public class GetQuestionsHandler: IRequestHandler<GetQuestionsQuery, IAsyncEnumerable<Question>>
    {
        private readonly EfCoreQuestionsRepository _questionsRepository;

        public GetQuestionsHandler(EfCoreQuestionsRepository questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }
        public Task<IAsyncEnumerable<Question>> Handle(GetQuestionsQuery request, CancellationToken cancellationToken)
        {
            var questions = _questionsRepository.GetQuestions(request.Tag);
            return Task.FromResult(questions);
        }

        
    }
}
