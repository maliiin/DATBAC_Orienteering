using MediatR;
using Microsoft.AspNetCore.Mvc;
using orienteering_backend.Core.Domain.Quiz.Pipelines;
using orienteering_backend.Core.Domain.Quiz.Dto;

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

        //greate new track
        //POST
        [HttpGet("getQuiz")]
        public async Task<QuizDto> GetQuiz(Guid QuizId)
        {
            var quizDto = await _mediator.Send(new GetQuiz.Request(QuizId));
            return quizDto;
        }

        [HttpPost("addQuizQuestion")]
        public async Task<bool> AddQuizQuestion(QuizQuestionDto quizQuestionDto, Guid quizId)
        {
            var status = await _mediator.Send(new AddQuizQuestion.Request(quizQuestionDto, quizId));
            return status;
        }

    }
}
