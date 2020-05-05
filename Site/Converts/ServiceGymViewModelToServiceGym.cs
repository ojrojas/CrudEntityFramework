using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace KallpaBox.Site.Converts
{
    public class ServiceGymViewModelToServiceGym : IMapper<ServiceGymViewModel, ServiceGym>
    {
        public void Map(ServiceGymViewModel source,ServiceGym  destination)
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