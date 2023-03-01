using MediatR;
using orienteering_backend.SharedKernel;

//Kilder: CampusEats events
//Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Products/Events/FoodItemNameChanged.cs (07.02.2023)
// bruker samme struktur som kilden
namespace orienteering_backend.Core.Domain.Checkpoint.Events;


public record QuizCheckpointCreated : IDomainEvent
{
    public QuizCheckpointCreated(Guid checkpointId, Guid quizId)
    {
        CheckpointId = checkpointId;
        QuizId = quizId;

    }

    public Guid CheckpointId { get; }
    //fix quizId er aldri null, men må sette den sånn
    public Guid QuizId { get; }
}
