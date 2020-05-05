using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Converts
{
    public class ServiceGymTypeViewModelToServiceGymType : IMapper<ServiceGymTypeViewModel, ServiceGymType>
    {
        public void Map(ServiceGymTypeViewModel source, ServiceGymType destination)
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
            destination.Type = source.Type;
        }
    }
}