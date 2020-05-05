using System.Collections.Generic;
using System.Linq;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class ClientService : IClientService
    {
        private readonly IRepository<Client> _clientRepository;

        public ClientService()
        {
            _clientRepository = new EfRepository<Client>(new GymContext());
        }

        public int CountClients()
        {
            var count =  _clientRepository.ListAll();
            return count.Count();
        }

        public void CreateClient(Client client)
        {
            _clientRepository.Add(client);
        }

        public void DeleteClient(int? clientId)
        {
            var entity =  _clientRepository.GetById(clientId);
             _clientRepository.Delete(entity);
        }

        public Client GetClientById(int? clientId)
        {
            var entity = _clientRepository.GetById(clientId);
            return entity;
        }

        public Client GetClientByIdentityGuid(string identityGuid)
        {
            var client =  _clientRepository.ListAll();
          return  client.Where(x => x.IdentityGuid == identityGuid).FirstOrDefault();
        }

        public IReadOnlyList<Client> GetListClient()
        {
            var listEntity =  _clientRepository.ListAll();
            return listEntity;
        }

        public void UpdateClient(Client client)
        {
            _clientRepository.Update(client);
        }
    }
}