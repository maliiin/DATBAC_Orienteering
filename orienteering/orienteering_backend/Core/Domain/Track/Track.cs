//using orienteering_backend.SharedKernel;

namespace orienteering_backend.Core.Domain.Track
{
    public class Track
    {
        public Track(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public Guid Id { get; protected set; }
        public Guid UserId { get; set; }

        public string Name { get; set; }
        public List<Checkpoint> CheckpointList { get; set; } = new();

        public void AddCheckpoint(Checkpoint checkpoint)
        {
                CheckpointList.Add(checkpoint);
        }
    }

}

    