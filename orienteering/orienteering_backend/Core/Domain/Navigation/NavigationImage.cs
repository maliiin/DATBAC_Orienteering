namespace orienteering_backend.Core.Domain.Navigation
{
    public class NavigationImage
    {
        public NavigationImage(string imagePath)
        {
            ImagePath = imagePath;
        }

        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public int Order { get; set; }
    }
}
