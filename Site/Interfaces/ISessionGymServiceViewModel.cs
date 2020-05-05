using System.Collections.Generic;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface ISessionGymServiceViewModel
    {
        void CreateSessionGym(SessionGymViewModel sessionGymViewModel);
        void UpateSessionGymView(int? id, SessionGymViewModel sessionGymViewModel);
        void DeleteSessionGymView(int? id);
        SessionGymViewModel GetSessionGymViewViewById(int? id);
        IReadOnlyList<SessionGymViewModel> GetAllSessionGymViewView();
        IReadOnlyList<SessionGymViewModel> GetAllSessionGymViewByClientIdView(int? clientId);
    }
}