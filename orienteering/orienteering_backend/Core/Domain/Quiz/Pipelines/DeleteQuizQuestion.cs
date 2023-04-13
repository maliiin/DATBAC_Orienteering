using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Pipelines;

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class DeleteQuizQuestion
{
    public record Request(
        Guid quizQuestionId, Guid quizId
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

            //check that user is allowed to access this quiz
            var trackUser = await _mediator.Send(new GetTrackUserByQuiz.Request(request.quizId));
            if (trackUser.UserId != userId)
            {
                //the user that owns the track is not the one signed in
                throw new NullReferenceException("the quiz dont exist or not allowed to access");
            }

            var quiz = await _db.Quiz
               .Where(q => q.Id == request.quizId)
               .Include(q => q.QuizQuestions)
               .FirstOrDefaultAsync(cancellationToken);


            if (quiz == null) { throw new NullReferenceException("the quiz dont exist or not allowed to access"); }

            //if quiz is null, the checkpoint did not have quiz
            //else delete the quiz

            quiz.RemoveQuizQuestion(request.quizQuestionId);
            await _db.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
