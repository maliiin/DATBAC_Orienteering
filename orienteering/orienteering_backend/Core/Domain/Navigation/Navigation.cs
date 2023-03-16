namespace orienteering_backend.Core.Domain.Navigation
{
    public class Navigation
    {
        public Navigation(Guid toCheckpoint, List<NavigationImage> images)
        {
            ToCheckpoint = toCheckpoint;
            Images = images;
        }

        public Guid Id { get; set; }
        //public Guid FromCheckpoint { get; set; }

        public Guid ToCheckpoint { get; set; }
        public List<NavigationImage> Images { get; set; } = new List<NavigationImage>();

        public int NumImages { get
            {
                return Images.Count;
            }
        }
    }
}
