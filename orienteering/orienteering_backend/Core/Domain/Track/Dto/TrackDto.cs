namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class TrackDto
    {
        public TrackDto() { 
        }
        public Guid? UserId { get; set; }
        public string? TrackName { get; set; }

        public Guid? TrackId { get; set; }
        public int NumCheckpoints { get; set; }

    }
}
