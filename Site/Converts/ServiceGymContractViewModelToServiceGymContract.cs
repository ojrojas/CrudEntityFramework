using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Converts
{
    public class ServiceGymContractViewModelToServiceGymContract : IMapper<ServiceGymContractViewModel, ServiceGymContract>
    {
        public void Map(ServiceGymContractViewModel source, ServiceGymContract destination)
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
            destination.Price = source.Price;
            destination.ServiceGym = source.ServiceGym;
            destination.ServiceGymId = source.ServiceGymId;
            destination.DateCelebrate = source.DateCelebrate;
            destination.DateExpiration = source.DateExpiration;
            destination.Client = source.Client;
            destination.ClientId = source.ClientId;
            destination.Quantity = source.Quantity;
            destination.TypeQuantity = source.TypeQuantity;
            destination.State = source.State;
        }
    }
}