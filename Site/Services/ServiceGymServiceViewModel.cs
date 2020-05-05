using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.ViewModels;
using Boxed.Mapping;
using Site.Interfaces;
using KallpaBox.Infraestructura.Logging;
using Site.Converts;
using KallpaBox.Site.Converts;

namespace Site.Services
{
    public class ServiceGymServiceViewModel : IServiceGymServiceViewModel
    {
        private readonly IServiceGymService _serviceGymRepository;
        private readonly IServiceGymTypeService _serviceGymTypeRepository;
#pragma warning disable CS0649 // El campo 'ServiceGymServiceViewModel._logger' nunca se asigna y siempre tendrá el valor predeterminado null
        private readonly IAppLogger<ServiceGymServiceViewModel> _logger;
#pragma warning restore CS0649 // El campo 'ServiceGymServiceViewModel._logger' nunca se asigna y siempre tendrá el valor predeterminado null
        private readonly IMapper<ServiceGymViewModel, ServiceGym> _converterServiceGymViewModelToServiceGym;
        private readonly IMapper<ServiceGym, ServiceGymViewModel> _converterServiceGymToServiceGymViewModel;


        public ServiceGymServiceViewModel()
        {
            _serviceGymRepository = new ServiceGymService();
            _converterServiceGymToServiceGymViewModel = new ServiceGymToServiceGymViewModel();
            _converterServiceGymViewModelToServiceGym = new ServiceGymViewModelToServiceGym();
            _serviceGymTypeRepository = new ServiceGymTypeService();
        }

        public void CreateServiceGymViewModel(ServiceGymViewModel serviceGymViewModel)
        {
            try
            {
                if (serviceGymViewModel != null)
                {
                    var serviceGym = _converterServiceGymViewModelToServiceGym.Map(serviceGymViewModel);
                     _serviceGymRepository.CreateServiceGym(serviceGym);
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

        public void DeleteServiceGymViewModel(int? id, ServiceGymViewModel serviceGymViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGym =  _serviceGymRepository.GetServiceGymById(id);
                if (serviceGym == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                 _serviceGymRepository.DeleteServiceGym(id);
                //_logger.LogInformation("ServiceGym Eliminado");

            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                //_logger.LogWarning("Error al eliminar el serviceGym, message:" + e.Message);
                throw new Exception("Error de excepcion al eliminar el service");
            }
        }

        public IReadOnlyList<ServiceGymViewModel> GetAllServiceGymViewModel()
        {
            try
            {
                var listServiceGym = new List<ServiceGymViewModel>();
                var list =  _serviceGymRepository.ListAllServiceGym();
                foreach (var i in list)
                {
                    var serviceGym = _converterServiceGymToServiceGymViewModel.Map(i);
                    listServiceGym.Add(serviceGym);
                }

                return listServiceGym;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("error al traer la lista de servicios, message:", e.Message);
                throw new Exception("Error al traer la lista de servicegym, message:" + e.Message);
            }
        }

        public ServiceGymViewModel GetServiceGymByIdViewModel(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGym =  _serviceGymRepository.GetServiceGymById(id);
                serviceGym.ServiceGymType =  _serviceGymTypeRepository.GetServiceGymTypeById(serviceGym.ServiceGymTypeId);
                if (serviceGym == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                return _converterServiceGymToServiceGymViewModel.Map(serviceGym);

            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver el serviceGym, message:" + e.Message);
                throw new Exception("Error al devolver el serviceGym, message:" + e.Message);
            }

        }

        public IReadOnlyList<ServiceGymTypeViewModel> GetServiceGymTypes()
        {
            try
            {
                var listServiceGymType = _serviceGymTypeRepository.ListAllServieGymTypeService();
                var list = new List<ServiceGymTypeViewModel>()
                {
                   // new ServiceGymTypeServiceViewModel() {Value = null,Text = "Selected ServiceType", Selected = true }
                };
                foreach (var i in listServiceGymType)
                {
                    list.Add(new ServiceGymTypeViewModel()
                    {
                        Id = int.Parse( i.Id.ToString()),
                        Type = i.Type
                    });
                }

                return list;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver la lista serviceGymType, message:" + e.Message);
                throw new Exception("Error al devolver la lista serviceGymType, message:" + e.Message);
            }
        }

        public void UpateServiceGymViewModel(int? id, ServiceGymViewModel serviceGymViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var serviceGym =  _serviceGymRepository.GetServiceGymById(id);
                if (serviceGym == null)
                {
                    _logger.LogWarning("Error al traer el service Gym");
                    throw new Exception("Error en la obtencion del service Gym");
                }

                _converterServiceGymViewModelToServiceGym.Map(serviceGymViewModel, serviceGym);
                 _serviceGymRepository.UpdateServiceGym(serviceGym);
                //_logger.LogInformation("ServiceGym Actualizado");
            }
            catch (Exception e)
            {
                //_logger.LogWarning("exception al actualizar el cliente especifico");
                throw new Exception("Error al actualizar el cliente. message: " + e.Message);
            }
        }
    }
}