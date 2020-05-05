using System.Collections.Generic;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface IClientServiceViewModel
    {
        int CreateClient(ClientViewModel clientViewModel);
        void UpateClient(int? id, ClientViewModel clientViewModel);
        void DeleteClient(int? id, ClientViewModel clientViewModel);
        ClientViewModel GetClientViewModelById(int? id);
        IReadOnlyList<ClientViewModel> GetAllClientViewModel();
        ClientViewModel GetClientViewModelByIdentityGuid(string identityGuid);
    }
}