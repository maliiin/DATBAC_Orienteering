namespace orienteering_backend.Core.Domain.Track
{
    public class Track
    {
        public int Id { get; set; }
        public List<Checkpoint>? CheckpointList { get; set; }
    }
}
