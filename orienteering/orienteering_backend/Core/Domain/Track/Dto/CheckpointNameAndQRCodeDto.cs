namespace orienteering_backend.Core.Domain.Track.Dto
{
    public class CheckpointNameAndQRCodeDto
    {

        public Guid Id { get; set; }    
        public byte[]? QRCode { get; set; }
    }
}
