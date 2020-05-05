using System.Collections.Generic;
using System.Linq;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class ServiceGymContractService : IServiceGymContractService
    {
        private readonly IRepository<ServiceGymContract> _serviceGymContractRepository;
        public ServiceGymContractService()
            => _serviceGymContractRepository = new EfRepository<ServiceGymContract>(new GymContext());

        public int CountServiceGymContracts()
        {
            var counts = _serviceGymContractRepository.ListAll();
            return counts.Count();
        }

        public void CreateServiceGymContracts(ServiceGymContract serviceGymContracts)
        {
            _serviceGymContractRepository.Add(serviceGymContracts);
        }

        public void DeleteServiceGymContracts(int? serviceContractsGymId)
        {
            var entity = _serviceGymContractRepository.GetById(serviceContractsGymId);
            _serviceGymContractRepository.Delete(entity);
        }

        public ServiceGymContract GetServiceContractClientByClientId(int? idClient)
        {
            var contracts = _serviceGymContractRepository.ListAll();
            return contracts.Where(x => x.ClientId == idClient && x.State == true).FirstOrDefault();
        }

        public ServiceGymContract GetServiceGymContractsById(int? serviceContractsGymId)
        {
            return _serviceGymContractRepository.GetById(serviceContractsGymId);
        }

        public IReadOnlyList<ServiceGymContract> ListAllServiceGymContracts()
        {
            return _serviceGymContractRepository.ListAll();
        }

        public void UpdateServiceGymContracts(ServiceGymContract serviceGymContracts)
        {
            _serviceGymContractRepository.Update(serviceGymContracts);
        }
    }
}