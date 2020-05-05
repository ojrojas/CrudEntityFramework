using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.Interfaces;
using Site.ViewModels;
using System.Linq;
using Boxed.Mapping;
using Site.Converts;

namespace Site.Services
{
    public class ServiceGymTypeServiceViewModel : IServiceGymTypeServiceViewModel
    {
        private readonly IServiceGymTypeService _serviceGymTypeRepository;
        private readonly IMapper<ServiceGymTypeViewModel, ServiceGymType> _converterServiceGymTypeViewModelToServiceGymType;
        private readonly IMapper<ServiceGymType, ServiceGymTypeViewModel> _converterServiceGymTypeToServiceGymTypeViewModel;


        public ServiceGymTypeServiceViewModel()
        {
            _serviceGymTypeRepository = new ServiceGymTypeService();
            _converterServiceGymTypeToServiceGymTypeViewModel = new ServiceGymTypeToServiceGymTypeViewModel();
            _converterServiceGymTypeViewModelToServiceGymType = new ServiceGymTypeViewModelToServiceGymType();
        }

        public void CreateServiceGymTypeViewModel(ServiceGymTypeViewModel serviceGymTypeViewModel)
        {
            try
            {
                if (serviceGymTypeViewModel != null)
                {
                    var serviceGymType = _converterServiceGymTypeViewModelToServiceGymType.Map(serviceGymTypeViewModel);
                     _serviceGymTypeRepository.CreateServiceGymType(serviceGymType);
                    //_logger.LogInformation("Service Gym Created");
                }
            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                //_logger.LogWarning("Error el modelo llega nulo, message: " + e.Message);
            }
        }

        public void DeleteServiceGymTypeViewModel(int? id, ServiceGymTypeViewModel serviceGymTypeViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymType =  _serviceGymTypeRepository.GetServiceGymTypeById(id);
                if (serviceGymType == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                 _serviceGymTypeRepository.DeleteServiceGymType(id);
                //_logger.LogInformation("ServiceGymType Eliminado");
            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                //_logger.LogWarning("Error al eliminar el serviceGymType, message:" + e.Message);
                throw new Exception("Error de excepcion al eliminar el service");
            }
        }

        public  IReadOnlyList<ServiceGymTypeViewModel> GetAllServiceGymTypeViewModel()
        {
            try
            {
                var listServiceGymType = new List<ServiceGymTypeViewModel>();
                var list =  _serviceGymTypeRepository.ListAllServieGymTypeService();
                list.AsEnumerable();
                foreach (var i in list)
                {
                    var serviceGymType = _converterServiceGymTypeToServiceGymTypeViewModel.Map(i);
                    listServiceGymType.Add(serviceGymType);
                }

                return listServiceGymType;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("error al traer la lista de servicios, message:", e.Message);
                throw new Exception("Error al traer la lista de servicegymType, message:" + e.Message);
            }
        }

        public ServiceGymTypeViewModel GetServiceGymTypeByIdViewModel(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymType =  _serviceGymTypeRepository.GetServiceGymTypeById(id);
                if (serviceGymType == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                return _converterServiceGymTypeToServiceGymTypeViewModel.Map(serviceGymType);

            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver el serviceGymType, message:" + e.Message);
                throw new Exception("Error al devolver el serviceGymType, message:" + e.Message);
            }

        }

        public void UpateServiceGymTypeViewModel(int? id, ServiceGymTypeViewModel serviceGymTypeViewModel)
        {
           try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var serviceGymType =  _serviceGymTypeRepository.GetServiceGymTypeById(id);
                if (serviceGymType == null)
                {
                    //_logger.LogWarning("Error al traer el service Gym");
                    throw new Exception("Error en la obtencion del service Gym");
                }

               _converterServiceGymTypeViewModelToServiceGymType.Map(serviceGymTypeViewModel,serviceGymType);
                 _serviceGymTypeRepository.UpdateServiceGymType(serviceGymType);
                //_logger.LogInformation("ServiceGymType Actualizado");
            }
            catch (Exception e)
            {
                //_logger.LogWarning("exception al actualizar el cliente especifico");
                throw new Exception("Error al actualizar el cliente. message: " + e.Message);
            }
        }
    }
}