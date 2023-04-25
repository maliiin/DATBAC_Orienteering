using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using AutoMapper;
using orienteering_backend.Core.Domain.Authentication.Services;
using orienteering_backend.Core.Domain.Track.Pipelines;
using System.Security.Authentication;

// License MediatR (Apache 2.0): https://github.com/jbogard/MediatR/blob/master/LICENSE
// License Automapper (MIT): https://github.com/AutoMapper/AutoMapper/blob/master/LICENSE.txt
// License .NET EFCore (MIT) https://github.com/dotnet/efcore/blob/main/LICENSE.txt


namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetQuiz
{
    public record Request(
        Guid QuizId
        ) : IRequest<QuizDto>;


    public class Handler : IRequestHandler<Request, QuizDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IIdentityService _identityService;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMapper mapper, IIdentityService identityService, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _identityService = identityService;
            _mediator = mediator;
            
        }
        public async Task<QuizDto> Handle(Request request, CancellationToken cancellationToken)
        {
            //check that signed in
            var userId = _identityService.GetCurrentUserId();
            if (userId == null) { throw new AuthenticationException("user not signed in"); }

            //check that user is allowed to access this quiz
            var trackUser = await _mediator.Send(new GetTrackUserByQuiz.Request(request.QuizId));
            if (trackUser.UserId != userId)
            {
                //the user that owns the track is not the one signed in
                throw new ArgumentNullException("the quiz is not owned by that user or does not exists");
            }

            //get quiz
            var quiz = await _db.Quiz
                .Include(q => q.QuizQuestions)
                .ThenInclude(a => a.Alternatives)
                .FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);

            if (quiz == null) { throw new ArgumentNullException("the quiz is not owned by that user or does not exists"); }

            var dtoList = new List<QuizQuestionDto>();
            for (var i = 0; i < quiz.QuizQuestions.Count; i++)
            {
                var quizQuestion = quiz.QuizQuestions[i];
                var quizQuestionDto = _mapper.Map<QuizQuestion, QuizQuestionDto>(quizQuestion);
                dtoList.Add(quizQuestionDto);
            }
            var quizDto = new QuizDto(quiz.Id, dtoList);
            return quizDto;
        }
    }

}
