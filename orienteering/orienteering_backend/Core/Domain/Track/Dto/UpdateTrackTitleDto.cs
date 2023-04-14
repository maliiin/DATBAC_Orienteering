namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class UpdateTrackTitleDto
    {
        public UpdateTrackTitleDto(Guid trackId, string newTitle)
        {
            TrackId = trackId;
            NewTitle = newTitle;
        }

        public Guid TrackId { get; set; }
        public string NewTitle { get; set; }
    }
}
