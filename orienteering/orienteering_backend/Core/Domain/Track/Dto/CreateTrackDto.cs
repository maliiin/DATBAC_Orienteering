namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class CreateTrackDto
    {
        public CreateTrackDto(string trackName)
        {
            TrackName = trackName;
        }

        public string TrackName { get; set; }


    }
}
