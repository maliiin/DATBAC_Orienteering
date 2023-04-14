using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
using AutoMapper;
using orienteering_backend.Core.Domain.Checkpoint.Pipelines;

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetQuizByCheckpointId
{
    public record Request(
        Guid CheckpointId
        ) : IRequest<QuizDto>;


    public class Handler : IRequestHandler<Request, QuizDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;


        public Handler(OrienteeringContext db, IMapper mapper, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper;
            _mediator = mediator;
        }
        public async Task<QuizDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var quizId = await _mediator.Send(new GetQuizIdOfCheckpoint.Request(request.CheckpointId));
            if (quizId == null) { throw new NullReferenceException("Checkpoint dont have quiz"); }

            //get quiz
            var quiz = await _db.Quiz
                .Include(q => q.QuizQuestions)
                .ThenInclude(a => a.Alternatives)
                .FirstOrDefaultAsync(q => q.Id == quizId, cancellationToken);

            if (quiz == null) { throw new NullReferenceException("Quiz not found"); }

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
