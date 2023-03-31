using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Quiz.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Authentication;
using Microsoft.AspNetCore.Authorization;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Quiz.Events;

namespace orienteering_backend.Controllers
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizController(IMediator Mediator)
        {
            _mediator = Mediator;
        }
        //bør userGuid sendes inn fra frontend? eller skal backend hente userId fra seg selv fra den som er logget inn?

        [HttpGet("getQuiz")]
        public async Task<QuizDto> GetQuiz(string quizId)
        {
            var QuizId = new Guid(quizId);
            var quizDto = await _mediator.Send(new GetQuiz.Request(QuizId));
            return quizDto;
        }

        [HttpPost("addQuizQuestion")]
        public async Task<ActionResult> AddQuizQuestion(InputCreateQuestionDto inputQuizQuestions)
        {

            var status = await _mediator.Send(new AddQuizQuestion.Request(inputQuizQuestions));
            Console.WriteLine(status);
            if (!status) {
                //fiks retur type
                return Unauthorized("something went wrong creating quiz question");
                //return NotFound("Could not add Quiz question"); 
            };
            //fiks returtypen her!!!
            return Created("Added quiz question.", null);
        }

        [HttpDelete("deleteQuestion")]
        public async Task DeleteQuestion(string questionId, string quizId)
        {
            Console.WriteLine("\n\n\n\nn\n\n delete!!!");
            //fix er dette ok navn på event?? det har jo ikke blitt slettet enda
            Guid questionGuid=new Guid(questionId);
            Guid quizGuid = new Guid(quizId);

            //fix sjekk at det er rett bruker som er logget inn for dette

            await _mediator.Publish(new QuizQuestionDeleted(quizGuid, questionGuid));

        }
        //[HttpGet("getNextQuizQuestion")]
        //public async Task<NextQuizQuestionDto> GetNextQuizQuestion(string quizId, string quizQuestionIndex)
        //{
        //    var QuizId = new Guid(quizId);
        //    var QuizQuestionIndex = Int32.Parse(quizQuestionIndex);
        //    var nextQuizQuestionDto = await _mediator.Send(new GetNextQuizQuestion.Request(QuizId, QuizQuestionIndex));
        //    return nextQuizQuestionDto;
        //}
        // fix; fjern dette dersom pipeline ikke brukt

        //[HttpGet("getSolution")]
        //public async Task<string> getSolution(string quizId, string quizQuestionId)
        //{
        //    var QuizId = new Guid(quizId);
        //    var QuizQuestionId = new Guid(quizQuestionId);
        //    var solution = await _mediator.Send(new GetSolution.Request(QuizId, QuizQuestionId));
        //    return solution;
        //}
        // fix; fjern dette dersom pipeline ikke brukt

    }
}
