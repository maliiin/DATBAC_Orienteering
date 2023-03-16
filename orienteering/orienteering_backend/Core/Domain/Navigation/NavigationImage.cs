namespace orienteering_backend.Core.Domain.Navigation
{
    public class NavigationImage
    {
        public NavigationImage(string imagePath, int order)
        {
            ImagePath = imagePath;
            Order = order;
        }

        public Guid Id { get; set; }
        public string ImagePath { get; set; }
        public int Order { get; set; }
    }
}
