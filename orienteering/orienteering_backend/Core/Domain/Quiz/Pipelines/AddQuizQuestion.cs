using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class AddQuizQuestion
{
    public record Request(
        InputCreateQuestionDto inputCreateQuestionDto
        ) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _identityService = identityService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            Guid QuizGuid = new Guid(request.inputCreateQuestionDto.QuizId);

            //check that user is allowed to access this quiz
            var trackUser = await _mediator.Send(new GetTrackUserByQuiz.Request(QuizGuid));
            if (trackUser.UserId != userId)
            {
                //the user that owns the track is not the one signed in
                throw new NullReferenceException("the quiz dont exist or you are not allowed to edit");
            }

            var Quiz = await _db.Quiz
                .FirstOrDefaultAsync(q => q.Id == QuizGuid, cancellationToken);

            if (Quiz == null)
            {
                throw new NullReferenceException("the quiz dont exist or you are not allowed to edit");
            }

            var quizQuestion = new QuizQuestion();
            quizQuestion.Question = request.inputCreateQuestionDto.Question;
            quizQuestion.CorrectAlternative = request.inputCreateQuestionDto.CorrectAlternative;

            List<Alternative> alternatives = new List<Alternative>();
            foreach (var dto in request.inputCreateQuestionDto.Alternatives)
            {
                alternatives.Add(new Alternative(dto.Id, dto.Text));
                //alternatives.Append(new Alternative(dto.Text));
            }
            quizQuestion.Alternatives = alternatives;

            Quiz.AddQuizQuestion(quizQuestion);
            await _db.SaveChangesAsync();
          
            return true;
        }
    }
}
