using System.Collections.Generic;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface IServiceGymContractServiceViewModel
    {
        void CreateServiceGymContract(ServiceGymContractViewModel serviceGymContractViewModel);
        void UpateServiceGymContractView(int? id, ServiceGymContractViewModel serviceGymContractViewModel);
        void DeleteServiceGymContractView(int? id, ServiceGymContractViewModel serviceGymContractViewModel);
        ServiceGymContractViewModel GetServiceGymContractViewViewById(int? id);
        IReadOnlyList<ServiceGymContractViewModel> GetAllServiceGymContractViewView();
        IReadOnlyList<ServiceGymViewModel> GetAllServiceGyms();
        IReadOnlyList<ClientViewModel> GetAllClients();
        Client GetServiceGymContractClientViewModelById(int? clientID);
        ServiceGym GetServiceGymContractServiceGymModelById(int? serviceGymId);
        ServiceGymContract GetServiceGymContractServiceViewModelByClientId(int? clientId);
    }
}
