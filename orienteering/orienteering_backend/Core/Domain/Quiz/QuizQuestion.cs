namespace orienteering_backend.Core.Domain.Quiz
{
    public class QuizQuestion
    {
        public QuizQuestion() 
        { 
        }
        public Guid Id { get; private set; }
        public string? Question { get; set; }
        public List<Alternative> Alternatives { get; set; } = new();
        public int CorrectAlternative { get; set; }
        
    }
}
