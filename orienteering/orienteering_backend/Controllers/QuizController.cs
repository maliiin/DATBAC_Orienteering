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
        //fix/se på -bør userGuid sendes inn fra frontend? eller skal backend hente userId fra seg selv fra den som er logget inn?

        [HttpGet("getQuiz")]
        public async Task<ActionResult<QuizDto>> GetQuiz(string quizId)
        {
            var QuizId = new Guid(quizId);

            try
            {
                var quizDto = await _mediator.Send(new GetQuiz.Request(QuizId));
                return quizDto;
            }
            catch
            {
                return Unauthorized();
            }

        }

        [HttpPost("addQuizQuestion")]
        public async Task<ActionResult> AddQuizQuestion(InputCreateQuestionDto inputQuizQuestions)
        {
            try
            {
                //fix-sjekk status eller returner error hvis feil
                var status = await _mediator.Send(new AddQuizQuestion.Request(inputQuizQuestions));
                //fiks returtypen her!!!

                return Created("Added quiz question.", null);

            }
            catch
            {
                return Unauthorized("something went wrong creating quiz question");

            }
            
        }

        [HttpDelete("deleteQuestion")]
        public async Task<ActionResult> DeleteQuestion(string questionId, string quizId)
        {
            //fix er dette ok navn på event?? det har jo ikke blitt slettet enda
            Guid questionGuid = new Guid(questionId);
            Guid quizGuid = new Guid(quizId);

            ////fix sjekk at det er rett bruker som er logget inn for dette
            //før event i samme domain-nå pipeline
            //await _mediator.Publish(new QuizQuestionDeleted(quizGuid, questionGuid));

            try
            {
                await _mediator.Send(new DeleteQuizQuestion.Request(questionGuid, quizGuid));
                return Ok();
            }
            catch
            {
                return Unauthorized();
            }

        }
        [HttpGet("getNextQuizQuestion")]
        public async Task<NextQuizQuestionDto> GetNextQuizQuestion(string quizId, string quizQuestionIndex)
        {
            var QuizId = new Guid(quizId);
            var QuizQuestionIndex = Int32.Parse(quizQuestionIndex);
            var nextQuizQuestionDto = await _mediator.Send(new GetNextQuizQuestion.Request(QuizId, QuizQuestionIndex));
            return nextQuizQuestionDto;
        }

        [HttpGet("getSolution")]
        public async Task<string> getSolution(string quizId, string quizQuestionId)
        {
            var QuizId = new Guid(quizId);
            var QuizQuestionId = new Guid(quizQuestionId);
            var solution = await _mediator.Send(new GetSolution.Request(QuizId, QuizQuestionId));
            return solution;
        }

    }
}
