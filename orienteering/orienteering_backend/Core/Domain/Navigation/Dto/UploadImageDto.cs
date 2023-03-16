namespace orienteering_backend.Core.Domain.Navigation.Dto
{
    public class UploadImageDto
    {
        public IFormFile FormFile { get; set; }
        public string CheckpointId { get; set; }
    }

}
