namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class AlternativeDto
    {
        public AlternativeDto(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
