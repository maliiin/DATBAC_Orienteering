﻿namespace orienteering_backend.Core.Domain.Navigation
{
    public class Navigation
    {
        public Navigation(Guid toCheckpoint)
        {
            ToCheckpoint = toCheckpoint;
        }

        public Guid Id { get; private set; }
        public Guid ToCheckpoint { get; set; }

        //private set på denne? så den ikke kan modifiseres
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
            //fix-her burde order blitt satt istedenfor at den settes manuelt
            Images.Add(image);
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
