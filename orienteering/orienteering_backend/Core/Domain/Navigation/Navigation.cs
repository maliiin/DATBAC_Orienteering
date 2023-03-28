using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace orienteering_backend.Core.Domain.Navigation
{
    public class Navigation
    {
        public Navigation(Guid toCheckpoint)
        {
            ToCheckpoint = toCheckpoint;
        }
        public Guid Id { get; set; }
        //public Guid FromCheckpoint { get; set; }

        public Guid ToCheckpoint { get; set; }
        public List<NavigationImage> Images { get; set; } = new List<NavigationImage>();

        public int NumImages
        {
            get
            {
                return Images.Count;
            }
        }

        public void AddNavigationImage(NavigationImage image)
        {
            Images.Add(image);
            Console.WriteLine("her er bildet i addnavigation");
            Console.WriteLine(image.TextDescription);
        }

        public bool RemoveNavigationImage(NavigationImage image)
        {

            var result= Images.Remove(image);
            if (result)
            {
                foreach(var navImage in Images)
                {
                    if (navImage.Order > image.Order)
                    {
                        navImage.Order--;
                    }
                }
            }
            return result;


        }
    }
}
