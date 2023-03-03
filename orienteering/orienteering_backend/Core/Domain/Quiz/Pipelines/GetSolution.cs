using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class GetSolution
{
    public record Request(
        Guid quizId,
        Guid quizQuestionId
        ) : IRequest<string>;


    public class Handler : IRequestHandler<Request, string>
    {
        private readonly OrienteeringContext _db;

        public Handler(OrienteeringContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }
        public async Task<string> Handle(Request request, CancellationToken cancellationToken)
        {
            var quiz = await _db.Quiz.Include(q => q.QuizQuestions).ThenInclude(a => a.Alternatives).Where(q => q.Id == request.quizId).FirstOrDefaultAsync(cancellationToken);

            if (quiz == null)
            {
                throw new Exception("Quiz not found");
            }
            var quizQuestion = quiz.QuizQuestions.Find(x => x.Id == request.quizQuestionId);
            if (quizQuestion == null)
            {
                throw new Exception("QuizQuestion not found");
            }
            var solution = quizQuestion.Alternatives[quizQuestion.CorrectAlternative - 1];
            return solution.Text;
        }
    }

}
