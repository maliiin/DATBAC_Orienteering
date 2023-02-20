namespace orienteering_backend.Core.Domain.Quiz
{
    public class Alternative
    {
        public Alternative( int id, string text) {
            Id = id;
            Text = text;
        }
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
