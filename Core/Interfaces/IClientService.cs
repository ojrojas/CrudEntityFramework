using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IClientService
    {
        IReadOnlyList<Client> GetListClient();
        void CreateClient(Client client);
        void DeleteClient(int? clientId);
        void UpdateClient(Client client);
        Client GetClientById(int? clinetId);
        int CountClients();
        Client GetClientByIdentityGuid(string identityGuid);
    }
}