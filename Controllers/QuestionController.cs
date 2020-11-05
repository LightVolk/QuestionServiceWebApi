using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using QuestionServiceWebApi.CQRS.Queries;
using QuestionServiceWebApi.Models;

namespace QuestionServiceWebApi.Controllers
{
    public class QuestionController : Controller
    {
        private readonly IMediator _mediator;

        public QuestionController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpGet("{tag}")]
        public async Task<IActionResult> GetQuestions(string tag)
        {
            var questions = await _mediator.Send(new GetQuestionsQuery(tag));
            if (questions == null)
                return NotFound();
            return Ok(questions);
        }

        
    }
}
