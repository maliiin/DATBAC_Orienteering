namespace orienteering_backend.Core.Domain.Quiz
{
    public class Alternative
    {
        public Alternative(string text) {
            Text = text;
        }
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
