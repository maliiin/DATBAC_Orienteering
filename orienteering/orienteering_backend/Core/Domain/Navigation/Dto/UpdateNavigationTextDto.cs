namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class UpdateNavigationTextDto
    {
        public UpdateNavigationTextDto(Guid navigationId, string newText, Guid navigationImageId)
        {
            NavigationId = navigationId;
            NewText = newText;
            NavigationImageId = navigationImageId;
        }

        public Guid NavigationId { get; set; }
        public string NewText { get; set; }
        public Guid NavigationImageId { get; set; }

    }
}
