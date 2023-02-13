namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class TrackDto
    {
        public TrackDto(Guid userId, string trackName) { 
            UserId= userId;
            TrackName= trackName;
        }

        public Guid UserId { get; set; }
        public string TrackName { get; set; }
    }
}
