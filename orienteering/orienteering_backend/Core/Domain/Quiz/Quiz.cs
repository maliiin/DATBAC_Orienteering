namespace orienteering_backend.Core.Domain.Quiz
{
    public class Quiz
    {
        public Quiz(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public List<QuizQuestion> QuizQuestions { get; set; } = new();

        public void AddQuizQuestion(QuizQuestion quizQuestion)
        {
            QuizQuestions.Add(quizQuestion);
        }

    }
}
