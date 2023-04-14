namespace orienteering_backend.Core.Domain.Quiz.Dto

{
    public class QuizDto
    {
        public QuizDto(Guid quizId, List<QuizQuestionDto> quizQuestions) {
            QuizId = quizId;
            QuizQuestions = quizQuestions;
        }
        public Guid QuizId { get; set; }
        public List<QuizQuestionDto> QuizQuestions { get; set; }
    }
}
