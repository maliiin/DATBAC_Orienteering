namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class NextQuizQuestionDto
    {
        public NextQuizQuestionDto(bool endOfQuiz)
        {
            EndOfQuiz = endOfQuiz;
        }
        public Guid QuizQuestionId { get; set; }
        public string? Question { get; set; }
        public List<Alternative>? Alternative { get; set; } = new();

        public int? CorrectAlternative { get; set; }
        public bool EndOfQuiz { get; set; }

    }
}
