namespace orienteering_backend.Core.Domain.Track
{
    public class Checkpoint
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public int? QuizId { get; set; }
        public int? GameId { get; set; }
    }
}
