namespace orienteering_backend.Core.Domain.Checkpoint.Dto
{
    public class CheckpointDto
    {
        public CheckpointDto(string title, Guid trackId, int gameId=0)
        {
            Title = title;
            TrackId = trackId;
            GameId = gameId;
        }

        public Guid? Id { get; set; }
        public string Title { get; set; }
        public Guid TrackId { get; set; }

        public int GameId { get; set; }
        public Guid? QuizId { get; set; }
        public int Order { get; set; }



    }
}
