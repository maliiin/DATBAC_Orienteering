namespace orienteering_backend.Core.Domain.Quiz
{
    public class QuizQuestion
    {
        public QuizQuestion(string question, int correctOption) 
        { 
            Question= question;
            CorrectOption= correctOption;
        }
        public Guid Id { get; set; }
        public string Question { get; set; }
        public List<Option> Options { get; set; } = new();
        public int CorrectOption { get; set; }
        
    }
}
