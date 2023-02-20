namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class InputCreateQuestionDto
    {
        public InputCreateQuestionDto(string question, List<AlternativeDto> alternatives, int correctAlternative, Guid quizId)
        {
            Question = question;
            Alternatives = alternatives;
            CorrectAlternative = correctAlternative;
            QuizId = quizId;
        }

        public string Question { get; set; }
        public List<AlternativeDto> Alternatives { get; set; } = new();
        public int CorrectAlternative { get; set; }
        public Guid QuizId { get; set; }


    }
}
