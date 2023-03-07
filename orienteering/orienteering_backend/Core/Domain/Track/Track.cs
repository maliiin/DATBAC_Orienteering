//using orienteering_backend.SharedKernel;
using orienteering_backend.Core.Domain.Checkpoint;

namespace orienteering_backend.Core.Domain.Track
{
    public class Track
    {
        public Track()
        {
        }

        public Guid Id { get; protected set; }
        public Guid? UserId { get; set; }

        public string? Name { get; set; }
        //public List<Guid> CheckpointList { get; set; } = new();

        //public void AddCheckpoint(Guid checkpointId)
        //{
        //        CheckpointList.Add(checkpointId);
        //}
    }

}

    