namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class UploadImageDto
    {
        public string? FileName { get; set; }    
        public IFormFile? FormFile { get; set; }
    }
}
