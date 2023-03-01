namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class QuizQuestionDto
    {
        public QuizQuestionDto(string question, int correctAlternative) {
            Question = question;
            CorrectAlternative = correctAlternative;

        }
        public Guid QuizQuestionId { get; set; }
        public string Question { get; set; }
        public List<Alternative> Alternative { get; set; } = new();

        public int CorrectAlternative { get; set; }
    }
}
