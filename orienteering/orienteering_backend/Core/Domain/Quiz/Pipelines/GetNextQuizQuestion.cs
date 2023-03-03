using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetNextQuizQuestion
{
    public record Request(
        Guid QuizId,
        int quizQuestionIndex
        ) : IRequest<NextQuizQuestionDto>;


    public class Handler : IRequestHandler<Request, NextQuizQuestionDto>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task<NextQuizQuestionDto> Handle(Request request, CancellationToken cancellationToken)
        {
            var Quiz = await _db.Quiz.Include(q => q.QuizQuestions).ThenInclude(a => a.Alternatives).FirstOrDefaultAsync(q => q.Id == request.QuizId, cancellationToken);
            if (Quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            if (Quiz.QuizQuestions.Count <= request.quizQuestionIndex)
            {
                return new NextQuizQuestionDto(true);
            }
            var quizQuestion = Quiz.QuizQuestions[request.quizQuestionIndex];
            var dtoElement = new NextQuizQuestionDto(false);
            dtoElement.Alternative = quizQuestion.Alternatives;
            dtoElement.QuizQuestionId = quizQuestion.Id;
            dtoElement.Question = quizQuestion.Question;
            // Sjekker om det er siste quizquestion i quiz
            if ((request.quizQuestionIndex + 1) == Quiz.QuizQuestions.Count)
            {
                dtoElement.EndOfQuiz = true;
            }
            return dtoElement;
        }
    }

}
