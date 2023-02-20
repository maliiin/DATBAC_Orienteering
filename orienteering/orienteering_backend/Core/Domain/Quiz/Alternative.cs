using System.ComponentModel.DataAnnotations;

namespace orienteering_backend.Core.Domain.Quiz
{
    public class Alternative
    {
        public Alternative(string text)
        {
            Text = text;
        }

        //public Alternative( int id, string text) {
        //    Id = id;
        //    Text = text;
        //}
        //test
        [Key]
        public Guid UniqueId { get; set; }


        public int Id { get; set; }
        public string Text { get; set; }
    }
}
