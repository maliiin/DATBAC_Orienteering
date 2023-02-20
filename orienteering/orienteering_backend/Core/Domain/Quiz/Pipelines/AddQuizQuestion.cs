using MediatR;
using Microsoft.EntityFrameworkCore;
using orienteering_backend.Infrastructure.Data;
using orienteering_backend.Core.Domain.Quiz.Dto;
//Kilder: CampusEats lab fra dat240
// Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Cart/Pipelines/AddItem.cs (07.02.2023)
// Brukte samme struktur på pipelinen som i kilden

namespace orienteering_backend.Core.Domain.Quiz.Pipelines;

public static class AddQuizQuestion
{
    public record Request(
        InputCreateQuestionDto inputCreateQuestionDto
        ) : IRequest<bool>;


    public class Handler : IRequestHandler<Request, bool>
    {
        private readonly OrienteeringContext _db;
        private readonly IMediator _mediator;

        public Handler(OrienteeringContext db, IMediator mediator)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mediator = mediator;
        }
        public async Task<bool> Handle(Request request, CancellationToken cancellationToken)
        {
            var Quiz = await _db.Quiz.FirstOrDefaultAsync(q => q.Id == request.inputCreateQuestionDto.QuizId, cancellationToken);
            if (Quiz == null)
            {
                return false;
            }
            var quizQuestion = new QuizQuestion(request.inputCreateQuestionDto.Question, request.inputCreateQuestionDto.CorrectAlternative);

            List<Alternative> alternatives = new List<Alternative>();
            foreach (var dto in request.inputCreateQuestionDto.Alternatives)
            {
                alternatives.Append(new Alternative(dto.Text));
            }
            quizQuestion.Alternatives = alternatives;
            Quiz.AddQuizQuestion(quizQuestion);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}
