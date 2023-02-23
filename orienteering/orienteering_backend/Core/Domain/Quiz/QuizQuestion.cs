namespace orienteering_backend.Core.Domain.Quiz
{
    public class QuizQuestion
    {
        public QuizQuestion(string question, int correctAlternative) 
        { 
            Question= question;
            CorrectAlternative= correctAlternative;
        }
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<Alternative> Alternatives { get; set; } = new();
        public int CorrectAlternative { get; set; }
        
    }
}
