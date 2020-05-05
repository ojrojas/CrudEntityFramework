using System.Collections.Generic;
using System.Linq;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class SessionGymService : ISessionGymService
    {
        private readonly IRepository<SessionGym> _sessionGymServiceRepository;
        public SessionGymService() 
            => _sessionGymServiceRepository=new EfRepository<SessionGym>(new GymContext());

        public int CountSessionGym()
        {
            var Count =  _sessionGymServiceRepository.ListAll();
            return Count.Count;
        }

        public void CreateSessionGym(SessionGym sessionGym)
        {
             _sessionGymServiceRepository.Add(sessionGym);
        }

        public void DeleteSessionGym(int? sessionGymId)
        {
            var entity =  _sessionGymServiceRepository.GetById(sessionGymId);
             _sessionGymServiceRepository.Delete(entity);
        }

        public  SessionGym GetSessionGymById(int? sessionGymId)
        {
            return  _sessionGymServiceRepository.GetById(sessionGymId);
        }

        public  IReadOnlyList<SessionGym> GetSessionsGymByClientId(int? clientId)
        {
            return (from i in  _sessionGymServiceRepository.ListAll() where i.ClientId == clientId select i).ToList();
        }

        public  IReadOnlyList<SessionGym> ListAllSessionGym()
        {
            return  _sessionGymServiceRepository.ListAll();
        }

        public  void UpdateSessionGym(int? sessionGymId, SessionGym sessionGym)
        {
            var entity =  _sessionGymServiceRepository.GetById(sessionGymId);
             _sessionGymServiceRepository.Update(entity);
        }
    }
}