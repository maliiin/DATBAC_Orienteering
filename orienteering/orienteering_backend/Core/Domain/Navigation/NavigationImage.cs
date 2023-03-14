namespace orienteering_backend.Core.Domain.Navigation
{
    public class NavigationImage
    {
        public NavigationImage(byte[] image)
        {
            Image = image;
        }

        public Guid Id { get; set; }
        public byte[] Image { get; set; }
    }
}
