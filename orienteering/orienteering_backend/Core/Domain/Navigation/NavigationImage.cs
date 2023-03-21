namespace orienteering_backend.Core.Domain.Navigation
{
    public class NavigationImage
    {
        public NavigationImage(string imagePath, int order, string textDescription)
        {
            ImagePath = imagePath;
            Order = order;
            TextDescription = textDescription;
        }

        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public string TextDescription { get; set; }
        public int Order { get; set; }
    }
}
