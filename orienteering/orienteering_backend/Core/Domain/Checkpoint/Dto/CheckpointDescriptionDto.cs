namespace orienteering_backend.Core.Domain.Checkpoint.Dto
{
    public class CheckpointDescriptionDto
    {
        public CheckpointDescriptionDto(Guid? checkpointId, string description)
        {
            CheckpointId = checkpointId;
            Description = description;
        }

        public Guid? CheckpointId { get; set; }
        public string Description { get; set; }
    }
}
