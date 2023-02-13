//using orienteering_backend.SharedKernel;

namespace orienteering_backend.Core.Domain.Track
{
    public class Checkpoint
    {
        public Checkpoint(string? title)
        {
            Title = title;
        }

        public Guid Id { get; set; }
        public string? Title { get; set; }
        public int? QuizId { get; set; }
        public int? GameId { get; set; }
        public byte[]? QRCode { get; set; }
    }
}
