namespace orienteering_backend.Core.Domain.Quiz.Dto
{
    public class OptionDto
    {
        public OptionDto(string text)
        {
            Text = text;
        }

        public string Text { get; set; }
    }
}
