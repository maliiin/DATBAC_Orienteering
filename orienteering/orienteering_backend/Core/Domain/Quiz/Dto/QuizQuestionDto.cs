namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class QuizQuestionDto
    {
        public QuizQuestionDto() {
        }
        public Guid? QuizQuestionId { get; set; }
        public string? Question { get; set; }
        public List<AlternativeDto> Alternatives { get; set; } = new();

        public int CorrectAlternative { get; set; } 
    }
}
