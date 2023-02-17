using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetQuiz
{
    public record Request(
        Guid QuizId
        ) : IRequest<QuizDto>;


    public class Handler : IRequestHandler<Request, QuizDto>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<QuizDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var Quiz = await _db.Quiz.Include(q => q.QuizQuestions).FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);
            if (Quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            var dtoList = new List<QuizQuestionDto>();
            for (var i = 0; i < Quiz.QuizQuestions.Count; i++)
            {
                var quizQuestion = Quiz.QuizQuestions[i];
                var dtoElement = new QuizQuestionDto(quizQuestion.Question, quizQuestion.CorrectOption);
                dtoElement.Options = quizQuestion.Options;
                dtoList.Add(dtoElement);
            }
            var quizDto = new QuizDto(Quiz.Id, dtoList);
            return quizDto;
        }
    }

}
