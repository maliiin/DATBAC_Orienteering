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
        //fiks
        //bytt navn på option i db set (til alternative elns)
        //fiks at du ikke kan lage quiz hvis du har valgt spill
        //fiks hardkoding av quiz id i addQuizQuestion


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


        [HttpGet("test")]
        public async Task<IdentityUser> TestGet(string k)
        {
            Console.WriteLine($"Test {k}");
            Console.WriteLine("ddjdjjdjd\n\n\n\n\n\n\n");

            var t= new IdentityUser();
            t.UserName= "test111111111111111";
            return t;
        }

    }
}
