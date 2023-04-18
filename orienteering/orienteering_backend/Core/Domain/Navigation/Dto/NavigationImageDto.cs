namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class NavigationImageDto
    {
        public NavigationImageDto(int order)
        {
            Order = order;
        }

        public byte[] ImageData { get; set; }
        public int Order { get; set; }
        public string TextDescription { get; set; }
        public string fileType { get; set; }
        public Guid Id { get; set; }

    }
}
