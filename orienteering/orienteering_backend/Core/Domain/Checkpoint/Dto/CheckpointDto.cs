namespace orienteering_backend.Core.Domain.Checkpoint.Dto
{
    public class CheckpointDto
    {
        public CheckpointDto(string title, Guid trackId)
        {
            Title = title;
            TrackId = trackId;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public Guid TrackId { get; set; }

    }
}
