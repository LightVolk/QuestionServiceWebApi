using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using QuestionServiceWebApi.Models;

namespace QuestionServiceWebApi.CQRS.Queries
{
    public class GetQuestionsQuery: IRequest<IAsyncEnumerable<Question>>
    {
        public string Tag { get; }

        public GetQuestionsQuery(string tag)
        {
            Tag = tag;
        }
    }
}
