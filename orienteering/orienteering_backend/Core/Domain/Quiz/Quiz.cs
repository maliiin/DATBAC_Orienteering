namespace orienteering_backend.Core.Domain.Quiz
{
    public class Quiz
    {
        public Guid Id { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; } = new();

        public void AddQuizQuestion(QuizQuestion quizQuestion)
        {
            QuizQuestions.Add(quizQuestion);
        }

    }
}
