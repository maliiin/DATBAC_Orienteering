namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class InputCreateQuestionDto
    {
        public InputCreateQuestionDto(string question, List<OptionDto> options, int correctOption, Guid quizId)
        {
            Question = question;
            Options = options;
            CorrectOption = correctOption;
            QuizId = quizId;
        }

        public string Question { get; set; }
        public List<OptionDto> Options { get; set; } = new();
        public int CorrectOption { get; set; }
        public Guid QuizId { get; set; }


    }
}
