namespace orienteering_backend.Core.Domain.Track
{
    public class Track
    {
        public Track()
        {
        }
        public Guid Id { get; protected set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public int NumCheckpoints { get; private set; } = 0;
        public void AddedCheckpoint()
        {
            NumCheckpoints++;
        }

        public bool RemovedCheckpoint() 
        {
            if(NumCheckpoints== 0)
            {
                return false;
            }
            NumCheckpoints--;
            return true;
        }
    }
}

    