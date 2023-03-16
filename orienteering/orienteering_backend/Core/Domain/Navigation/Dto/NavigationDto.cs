namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class NavigationDto
    {
        public Guid ToCheckpoint { get; set; }
        public List<NavigationImageDto> Images { get; set; } = new List<NavigationImageDto>();

    }
}
