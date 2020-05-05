using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IServiceGymContractService
    {
        IReadOnlyList<ServiceGymContract> ListAllServiceGymContracts();
        ServiceGymContract GetServiceGymContractsById(int? serviceContractsGymId);
        void CreateServiceGymContracts(ServiceGymContract serviceGymContracts);
        void UpdateServiceGymContracts(ServiceGymContract serviceGymContracts);
        void DeleteServiceGymContracts(int? serviceContractsGymId);
        int CountServiceGymContracts();
        ServiceGymContract GetServiceContractClientByClientId(int? idClient);
    }
}