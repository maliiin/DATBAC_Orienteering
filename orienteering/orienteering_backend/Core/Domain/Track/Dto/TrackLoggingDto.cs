namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class TrackLoggingDto
    {
        public Guid? StartCheckpointId { get; set; }
        public string? TimeUsed { get; set; }
        public int? Score { get; set; }
    }
}
