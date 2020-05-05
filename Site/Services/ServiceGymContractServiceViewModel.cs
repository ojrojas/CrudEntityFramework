using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.ViewModels;
using System.Linq;
using Boxed.Mapping;
using Site.Interfaces;
using KallpaBox.Site.Data;
using Site.Converts;

namespace Site.Services
{
    public class ServiceGymContractServiceViewModel : IServiceGymContractServiceViewModel
    {

        private readonly IServiceGymContractService _serviceGymContractRepository;
        private readonly IClientService _clientServiceRepository;
        private readonly IServiceGymService _serviceGymRespository;
        private readonly IMapper<ServiceGymContractViewModel, ServiceGymContract> _converterServiceGymContractViewModelToServiceGymContract;
        private readonly IMapper<ServiceGymContract, ServiceGymContractViewModel> _converterServiceGymContractToServiceGymContractViewModel;


        public ServiceGymContractServiceViewModel()
        {
            _serviceGymContractRepository = new ServiceGymContractService();
            _serviceGymRespository = new ServiceGymService();
            _clientServiceRepository = new ClientService();
            _converterServiceGymContractToServiceGymContractViewModel = new ServiceGymContractToServiceGymContractViewModel() ;
            _converterServiceGymContractViewModelToServiceGymContract = new ServiceGymContractViewModelToServiceGymContract() ;
        }

        public void CreateServiceGymContract(ServiceGymContractViewModel serviceGymContractViewModel)
        {
            try
            {
                if (serviceGymContractViewModel != null)
                {
                    var serviceGymContract = _converterServiceGymContractViewModelToServiceGymContract.Map(serviceGymContractViewModel);
                     _serviceGymContractRepository.CreateServiceGymContracts(serviceGymContract);
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

        public void DeleteServiceGymContractView(int? id, ServiceGymContractViewModel serviceGymContractViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymContract =  _serviceGymContractRepository.GetServiceGymContractsById(id);
                if (serviceGymContract == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                 _serviceGymContractRepository.DeleteServiceGymContracts(id);
                //_logger.LogInformation("ServiceGymContract Eliminado");

            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                //_logger.LogWarning("Error al eliminar el serviceGymContract, message:" + e.Message);
                throw new Exception("Error de excepcion al eliminar el service");
            }
        }

        public IReadOnlyList<ClientViewModel> GetAllClients()
        {
            try
            {
                var listClient =  _clientServiceRepository.GetListClient();
                var list = new List<ClientViewModel>()
                {
                   // new SelectListItem() {Value = null,Text = "Selected Client", Selected = true }
                };
                foreach (var i in listClient)
                {
                    list.Add(new ClientViewModel()
                    {
                        //Value = i.Id.ToString(),
                        //Text = i.Name.ToString() + " " + i.LastName + " " + i.SecondSurName
                    });
                }

                return list;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver la lista de clientes, message:" + e.Message);
                throw new Exception("Error al devolver la lista de clientes, message:" + e.Message);
            }
        }

        public IReadOnlyList<ServiceGymContractViewModel> GetAllServiceGymContractViewView()
        {
            try
            {
                var listServiceGymContract = new List<ServiceGymContractViewModel>();
                var list =  _serviceGymContractRepository.ListAllServiceGymContracts();
                list.AsEnumerable();
                foreach (var i in list)
                {
                    var serviceGymContract = _converterServiceGymContractToServiceGymContractViewModel.Map(i);
                    serviceGymContract.ServiceGym =  _serviceGymRespository.GetServiceGymById(serviceGymContract.ServiceGymId);
                    serviceGymContract.Client =  _clientServiceRepository.GetClientById(serviceGymContract.ClientId);
                    listServiceGymContract.Add(serviceGymContract);
                }

                return listServiceGymContract;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("error al traer la lista de servicios, message:", e.Message);
                throw new Exception("Error al traer la lista de servicegymContract, message:" + e.Message);
            }
        }

        public IReadOnlyList<ServiceGymViewModel> GetAllServiceGyms()
        {
            try
            {
                var listServiceGym =  _serviceGymRespository.ListAllServiceGym();
                var list = new List<ServiceGymViewModel>()
                {
                  //  new SelectListItem() {Value = null,Text = "Selected Service Gym", Selected = true }
                };
                foreach (var i in listServiceGym)
                {
                    list.Add(new ServiceGymViewModel()
                    {
                        //Value = i.Id.ToString(),
                        //Text = i.Name.ToString()
                    });
                }

                return list;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver la lista de Service Gyms, message:" + e.Message);
                throw new Exception("Error al devolver la lista de Service Gyms, message:" + e.Message);
            }
        }

        public Client GetServiceGymContractClientViewModelById(int? clientID)
        {
            var client =  _clientServiceRepository.GetClientById(clientID);
            return client;
        }

        public ServiceGym GetServiceGymContractServiceGymModelById(int? serviceGymId)
        {
            var serviceGym =  _serviceGymRespository.GetServiceGymById(serviceGymId);
            return serviceGym;
        }

        public ServiceGymContract GetServiceGymContractServiceViewModelByClientId(int? clientId)
        {
            return  _serviceGymContractRepository.GetServiceContractClientByClientId(clientId);
        }

        public ServiceGymContractViewModel GetServiceGymContractViewViewById(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymContract =  _serviceGymContractRepository.GetServiceGymContractsById(id);
                serviceGymContract.ServiceGym =  _serviceGymRespository.GetServiceGymById(serviceGymContract.ServiceGymId);
                serviceGymContract.Client =  _clientServiceRepository.GetClientById(serviceGymContract.ClientId);
                if (serviceGymContract == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                return _converterServiceGymContractToServiceGymContractViewModel.Map(serviceGymContract);

            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver el serviceGymContract, message:" + e.Message);
                throw new Exception("Error al devolver el serviceGymContract, message:" + e.Message);
            }

        }

        public void UpateServiceGymContractView(int? id, ServiceGymContractViewModel serviceGymContractViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }

                var serviceGymContract =  _serviceGymContractRepository.GetServiceGymContractsById(id);

                if (serviceGymContract == null)
                {
                    //_logger.LogWarning("Error al traer el service Gym");
                    throw new Exception("Error en la obtencion del service Gym");
                }

                _converterServiceGymContractViewModelToServiceGymContract.Map(serviceGymContractViewModel, serviceGymContract);
                 _serviceGymContractRepository.UpdateServiceGymContracts(serviceGymContract);
                //_logger.LogInformation("ServiceGymContract Actualizado");
            }
            catch (Exception e)
            {
                //_logger.LogWarning("exception al actualizar el cliente especifico");
                throw new Exception("Error al actualizar el service gym contract. message: " + e.Message);
            }
        }
    }
}