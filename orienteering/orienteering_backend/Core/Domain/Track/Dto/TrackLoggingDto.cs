namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class TrackLoggingDto
    {
        public TrackLoggingDto()
        {
        }

        public Guid? StartCheckpointId { get; set; }

        public DateTime? TimeUsed { get; set; }
    }
}
