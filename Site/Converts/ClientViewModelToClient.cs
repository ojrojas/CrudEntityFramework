using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Converts
{
    public class ClientViewModelToClient : IMapper<ClientViewModel,Client>
    {
        public void Map(ClientViewModel source, Client destination)
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
            destination.Identification = source.Identification;
            destination.IdentityGuid = source.IdentityGuid;
            destination.Name = source.Name;
            destination.MiddleName = source.MiddleName;
            destination.LastName = source.LastName;
            destination.SecondSurName = source.SecondSurName;
            destination.Address = source.Address;
            destination.Age = source.Age;
            destination.Phone = source.Phone;
        }
    }
}