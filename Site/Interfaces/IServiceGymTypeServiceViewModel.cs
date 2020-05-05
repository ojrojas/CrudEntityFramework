using System.Collections.Generic;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface IServiceGymTypeServiceViewModel
    {
        void CreateServiceGymTypeViewModel(ServiceGymTypeViewModel clientViewModel);
        void UpateServiceGymTypeViewModel(int? id, ServiceGymTypeViewModel ServiceGymTypeViewModel);
        void DeleteServiceGymTypeViewModel(int? id, ServiceGymTypeViewModel ServiceGymTypeViewModel);
        ServiceGymTypeViewModel GetServiceGymTypeByIdViewModel(int? id);
        IReadOnlyList<ServiceGymTypeViewModel> GetAllServiceGymTypeViewModel();
    }
}