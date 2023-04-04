namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class AlternativeDto
    {
        public AlternativeDto(string text, int id)
        {
            Text = text;
            Id = id;
        }

        public string Text { get; set; }
        public int Id { get; set; }
    }
}
