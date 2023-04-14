using MediatR;

//Kilder: https://github.com/pdevito3/domain-events-example/blob/f6009db2ef9531e91be48d8f0c6ff908dd59095a/RecipeManagement/src/RecipeManagement/Domain/BaseEntity.cs
// "lasted ned: 07.02.2023"
// bruker samme struktur som kilden

// Lisens MediatR: https://github.com/jbogard/MediatR/blob/master/LICENSE

namespace orienteering_backend.SharedKernel;

public interface IDomainEvent : INotification { }

public abstract class BaseEntity
{
    public List<IDomainEvent> DomainEvents { get; } = new List<IDomainEvent>();
}
