using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Interfaces
{
    public interface IServiceGymTypeService
    {
        IReadOnlyList<ServiceGymType> ListAllServieGymTypeService();
        ServiceGymType GetServiceGymTypeById(int? serviceTypeGymId);
        void CreateServiceGymType(ServiceGymType serviceGymType);
        void UpdateServiceGymType(ServiceGymType serviceGymType);
        void DeleteServiceGymType(int? serviceGymTypeId);
        int CountServiceGymType();
    }
}