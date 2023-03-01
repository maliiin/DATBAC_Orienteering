using MediatR;
using orienteering_backend.SharedKernel;
namespace orienteering_backend.Core.Domain.Quiz.Events;




public record QuizQuestionDeleted : IDomainEvent
{
    public QuizQuestionDeleted(Guid quizId, Guid questionId)
    {
        QuizId = quizId;
        QuestionId = questionId;

    }

    public Guid QuizId { get; }
    public Guid QuestionId { get; }
}
