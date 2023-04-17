namespace orienteering_backend.Core.Domain.Checkpoint.Dto
{
    public class UpdateCheckpointTitleDto
    {
        public UpdateCheckpointTitleDto(Guid checkpointId, string title)
        {
            CheckpointId = checkpointId;
            Title = title;
        }

        public Guid CheckpointId { get; set; }
        public string Title { get; set; }
        
    }
}
