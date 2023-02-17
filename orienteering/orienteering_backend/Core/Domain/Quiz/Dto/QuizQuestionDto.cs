namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class QuizQuestionDto
    {
        public QuizQuestionDto(string question, int correctOption) {
            Question = question;
            CorrectOption = correctOption;

        }
        public string Question { get; set; }
        public List<Option> Options { get; set; } = new();

        public int CorrectOption { get; set; }
    }
}
