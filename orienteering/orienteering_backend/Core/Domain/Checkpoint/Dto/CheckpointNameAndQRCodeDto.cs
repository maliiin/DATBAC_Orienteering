﻿namespace orienteering_backend.Core.Domain.Checkpoint.Dto
{
    public class CheckpointNameAndQRCodeDto
    {

        public Guid Id { get; set; }
        public byte[]? QRCode { get; set; }
        public string Title { get; set; }

    }
}
