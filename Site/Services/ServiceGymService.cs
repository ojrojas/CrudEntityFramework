using System.Collections.Generic;
using System.Threading.Tasks;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class ServiceGymService : IServiceGymService
    {
        private readonly IRepository<ServiceGym> _serviceGymRepository;
        public ServiceGymService() 
            => _serviceGymRepository = new EfRepository<ServiceGym>(new GymContext());

        public int CountServiceGym()
        {
            var Count =  _serviceGymRepository.ListAll();
            return Count.Count;
        }

        public  void CreateServiceGym(ServiceGym serviceGym)
        {
             _serviceGymRepository.Add(serviceGym);
        }

        public  void DeleteServiceGym(int? serviceGymId)
        {
            var entity =  _serviceGymRepository.GetById(serviceGymId);
             _serviceGymRepository.Delete(entity);
        }

        public ServiceGym GetServiceGymById(int? serviceGymId)
        {
            return  _serviceGymRepository.GetById(serviceGymId);
        }

        public  IReadOnlyList<ServiceGym> ListAllServiceGym()
        {
            return  _serviceGymRepository.ListAll();
        }

        public  void UpdateServiceGym(ServiceGym serviceGym)
        {
             _serviceGymRepository.Update(serviceGym);
        }
    }
}