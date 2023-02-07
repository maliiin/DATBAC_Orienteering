using MediatR;
using orienteering_backend.SharedKernel;

//Kilder: CampusEats events
//Kilder: https://github.com/dat240-2022/assignments/blob/main/Lab3/UiS.Dat240.Lab3/Core/Domain/Products/Events/FoodItemNameChanged.cs (07.02.2023)
// bruker samme struktur som kilden
namespace orienteering_backend.Core.Domain.Track.Events;


public record CheckpointCreated : IDomainEvent
{
    public CheckpointCreated(Guid checkpointId)
    {
        CheckpointId = checkpointId;
    }

    public Guid CheckpointId { get; }
}
