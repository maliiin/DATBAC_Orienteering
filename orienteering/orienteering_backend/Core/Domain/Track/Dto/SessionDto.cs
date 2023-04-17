namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class SessionDto
    {
        public SessionDto(Guid checkpointId)
        {
            CheckpointId = checkpointId;
        }

        public Guid CheckpointId { get; set; }
    }
}
