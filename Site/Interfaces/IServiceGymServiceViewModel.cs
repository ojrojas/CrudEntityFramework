using System.Collections.Generic;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface IServiceGymServiceViewModel
    {
        void CreateServiceGymViewModel(ServiceGymViewModel serviceGymViewModel);
        void UpateServiceGymViewModel(int? id, ServiceGymViewModel serviceGymViewModel);
        void DeleteServiceGymViewModel(int? id, ServiceGymViewModel serviceGymViewModel);
        ServiceGymViewModel GetServiceGymByIdViewModel(int? id);
        IReadOnlyList<ServiceGymViewModel> GetAllServiceGymViewModel();
        IReadOnlyList<ServiceGymTypeViewModel> GetServiceGymTypes();
    }
}
