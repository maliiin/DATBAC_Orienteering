using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using System.Security.Authentication;
using orienteering_backend.Core.Domain.Track.Pipelines;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class DeleteQuizQuestion
{
    public record Request(
        Guid quizQuestionId, Guid quizId
        ):IRequest<bool>;


    //fix- se på denne-har ingen returtype
    //hvis ok bør vi se på de andre som heller ikke trenger å returnere noe
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
                throw new AuthenticationException("the quiz is not owned by that user");
            }

            var quiz = await _db.Quiz
               .Where(q => q.Id == request.quizId)
               .Include(q=>q.QuizQuestions)
               .FirstOrDefaultAsync(cancellationToken);



            //if quiz is null, the checkpoint did not have quiz
            //else delete the quiz
            if (quiz != null)
            {
                quiz.RemoveQuizQuestion(request.quizQuestionId);
                await _db.SaveChangesAsync(cancellationToken);
            }
            //fix ingenting skjer hvis quiz er null??
            return true;
        }
    }
}
