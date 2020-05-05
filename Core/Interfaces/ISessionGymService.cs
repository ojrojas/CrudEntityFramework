using System.Collections.Generic;
using System.Threading.Tasks;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface ISessionGymService
    {
        IReadOnlyList<SessionGym> ListAllSessionGym();
        SessionGym GetSessionGymById(int? sessionGymId);
        void CreateSessionGym(SessionGym sessionGymId);
        void UpdateSessionGym(int? sessionGymId, SessionGym sessionGym);
        void DeleteSessionGym(int? sessionGymId);
        int CountSessionGym();
        IReadOnlyList<SessionGym> GetSessionsGymByClientId(int? clientId);
    }
}