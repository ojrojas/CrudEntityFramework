using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Converts
{
    public class ServiceGymToServiceGymViewModel : IMapper<ServiceGym, ServiceGymViewModel>
    {
        public void Map(ServiceGym source, ServiceGymViewModel destination)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (destination == null)
            {
                throw new ArgumentNullException(nameof(destination));
            }

            destination.Id = source.Id;
            destination.Name = source.Name;
            destination.ServiceGymTypeId = source.ServiceGymTypeId;
            destination.ServiceGymType = source.ServiceGymType;
        }
    }
}