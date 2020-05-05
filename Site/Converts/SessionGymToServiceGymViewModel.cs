using System;
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Converts
{
    public class SessionGymToSessionGymViewModel : IMapper<SessionGym, SessionGymViewModel>
    {
        public void Map(SessionGym source, SessionGymViewModel destination)
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
            destination.Client = source.Client;
            destination.ClientId = source.ClientId;
            destination.StartSession = source.StartSession;
            destination.EndSession = source.EndSession;
        }
    }
}