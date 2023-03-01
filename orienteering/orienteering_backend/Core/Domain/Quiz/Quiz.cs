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

        public bool RemoveQuizQuestion(Guid questionId)
        {
            Console.WriteLine("skal slette question i klasse");
            Console.WriteLine(questionId);

            var questionToDelete=QuizQuestions.SingleOrDefault(question=>question.Id== questionId);
            if (questionToDelete == null) { return false; };
            _ = QuizQuestions.Remove(questionToDelete);
            return true;
            

        }

    }
}
