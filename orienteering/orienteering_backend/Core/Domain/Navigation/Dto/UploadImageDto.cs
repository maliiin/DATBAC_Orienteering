namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class UploadImageDto
    {
        public string? fileName { get; set; }    
        public IFormFile? Image { get; set; }
    }
}
