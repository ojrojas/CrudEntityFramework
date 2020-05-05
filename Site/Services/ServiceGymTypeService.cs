using System.Collections.Generic;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class ServiceGymTypeService : IServiceGymTypeService
    {
        private readonly IRepository<ServiceGymType> _serviceGymTypeServiceRepository;
        public ServiceGymTypeService()
        {
            _serviceGymTypeServiceRepository = new EfRepository<ServiceGymType>(new GymContext());
        }

        public int CountServiceGymType()
        {
            var Count = _serviceGymTypeServiceRepository.ListAll();
            return Count.Count;
        }

        public void CreateServiceGymType(ServiceGymType serviceGymType)
        {
            _serviceGymTypeServiceRepository.Add(serviceGymType);
        }

        public void DeleteServiceGymType(int? serviceGymTypeId)
        {
            var entity = _serviceGymTypeServiceRepository.GetById(serviceGymTypeId);
            _serviceGymTypeServiceRepository.Delete(entity);
        }

        public ServiceGymType GetServiceGymTypeById(int? serviceTypeGymId)
        {
            return _serviceGymTypeServiceRepository.GetById(serviceTypeGymId);
        }

        public IReadOnlyList<ServiceGymType> ListAllServieGymTypeService()
        {
            return _serviceGymTypeServiceRepository.ListAll();
        }

        public void UpdateServiceGymType(ServiceGymType serviceGymType)
        {
            _serviceGymTypeServiceRepository.Update(serviceGymType);
        }
    }
}