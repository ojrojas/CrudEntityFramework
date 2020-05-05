using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IServiceGymService
    {
        IReadOnlyList<ServiceGym> ListAllServiceGym();
        ServiceGym GetServiceGymById(int? serviceGymId);
        void CreateServiceGym(ServiceGym serviceGym);
        void UpdateServiceGym(ServiceGym serviceGym);
        void DeleteServiceGym(int? serviceGymId);
        int CountServiceGym();
    }
}