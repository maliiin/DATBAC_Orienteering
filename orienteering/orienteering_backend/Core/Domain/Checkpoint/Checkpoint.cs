namespace orienteering_backend.Core.Domain.Checkpoint
{
    public class Checkpoint
    {
        public Checkpoint(string title, int gameId, Guid trackId)
        {
            Title = title;
            GameId = gameId;
            TrackId = trackId;
        }

        public Guid Id { get; private set; }
        public string Title { get; set; }
        public Guid? QuizId { get; set; }
        public int GameId { get; set; }
        public byte[]? QRCode { get; set; }
        public Guid TrackId { get; set; }
        public int Order { get; set; }
    }
}
