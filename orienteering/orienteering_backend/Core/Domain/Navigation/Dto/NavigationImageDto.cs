namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class NavigationImageDto
    {
        public NavigationImageDto(int order)
        {
            Order = order;
        }

        //fix går dette fint? ikke lik original
        public byte[] ImageData { get; set; }
        public int Order { get; set; }
    }
}
