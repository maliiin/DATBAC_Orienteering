using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Quiz
{
    public class Alternative
    {

        public Alternative(int id, string text)
        {
            Id = id;
            Text = text;
        }

        [Key]
        public Guid UniqueId { get; private set; }

        // Id = hvilket alternativnr i quizquestion
        public int Id { get; set; }
        public string Text { get; set; }
    }
}
