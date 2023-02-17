namespace orienteering_backend.Core.Domain.Quiz
{
    public class Option
    {
        public Option(string text) {
            Text = text;
        }
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
