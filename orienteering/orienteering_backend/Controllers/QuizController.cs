using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Quiz.Dto;
using System.Security.Authentication;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

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


        //fix-denne bør sikres
        [HttpGet("getQuiz")]
        public async Task<ActionResult<QuizDto>> GetQuiz(string quizId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var QuizId = new Guid(quizId);

            //try
            //{
            var quizDto = await _mediator.Send(new GetQuiz.Request(QuizId));
            return quizDto;
            //}
            //catch
            //{
            //    return Unauthorized();
            //}

        }

        [HttpGet("getQuizByCheckpoint")]
        public async Task<ActionResult> GetQuizByCheckpoint(string checkpointId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var CheckpointId = new Guid(checkpointId);
            try
            {
                var quiz = await _mediator.Send(new GetQuizByCheckpointId.Request(CheckpointId));

                return Ok(quiz);

            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpPost("addQuizQuestion")]
        public async Task<ActionResult> AddQuizQuestion(InputCreateQuestionDto inputQuizQuestions)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            try
            {
                //fix-sjekk status eller returner error hvis feil
                var status = await _mediator.Send(new AddQuizQuestion.Request(inputQuizQuestions));
                //fiks returtypen her!!!

                return Created("Added quiz question.", null);

            }
            catch (AuthenticationException)
            {
                return Unauthorized("user not signed in");

            }
            catch (NullReferenceException)
            {
                return NotFound();
            }
        }

        [HttpDelete("deleteQuestion")]
        public async Task<ActionResult> DeleteQuestion(string questionId)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            Guid questionGuid = new Guid(questionId);

            try
            {
                await _mediator.Send(new DeleteQuizQuestion.Request(questionGuid));
                return Ok();
            }
            catch (AuthenticationException)
            {
                return Unauthorized("user not signed in");
            }
            catch (NullReferenceException)
            {
                return NotFound("");
            }

        }
    }
}
