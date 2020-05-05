using Boxed.Mapping;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.Converts;
using Site.Interfaces;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Site.Services
{
    public class ClientServiceViewModel : IClientServiceViewModel
    {
        private readonly IClientService _clientRepository;
#pragma warning disable CS0169 // El campo 'ClientServiceViewModel._logger' nunca se usa
        private readonly IAppLogger<ClientServiceViewModel> _logger;
#pragma warning restore CS0169 // El campo 'ClientServiceViewModel._logger' nunca se usa
        private readonly IMapper<Client, ClientViewModel> _converterClientToClientViewModel;
        private readonly IMapper<ClientViewModel, Client> _converterClientViewModelToClient;


        public ClientServiceViewModel()
        {
            _clientRepository = new ClientService();
            _converterClientToClientViewModel = new ClientToClientViewModel();
            _converterClientViewModelToClient = new ClientViewModelToClient();
        }

        public int CreateClient(ClientViewModel clientViewModel)
        {
            try
            {
                if (clientViewModel != null)
                {
                    var client = _converterClientViewModelToClient.Map(clientViewModel);
                     _clientRepository.CreateClient(client);
                    return client.Id;
                    //_logger.LogInformation("Client Created");
                }

                return 0;
            }
#pragma warning disable CS0168 // La variable 'e' se ha declarado pero nunca se usa
            catch (Exception e)
#pragma warning restore CS0168 // La variable 'e' se ha declarado pero nunca se usa
            {
                return 0;
                //_logger.LogWarning("Exception CreateClient message:" + e.Message);
            }
        }

        public void DeleteClient(int? id, ClientViewModel clientViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var client =  _clientRepository.GetClientById(id);
                if (client == null)
                {
                    //_logger.LogWarning("Error en la obtencion del cliente");
                    throw new Exception("Error en la obtencion del cliente");
                }
                 _clientRepository.DeleteClient(id);
            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al eliminar el cliente, message: ");
                throw new Exception("Error al eliminar el cliente, message: " + e.Message);
            }
        }

        public IReadOnlyList<ClientViewModel> GetAllClientViewModel()
        {
            try
            {
                var listClientViewModel = new List<ClientViewModel>();
                var listClient =  _clientRepository.GetListClient();
                listClient.AsEnumerable();
                foreach (var i in listClient)
                {
                    var client = _converterClientToClientViewModel.Map(i);
                    listClientViewModel.Add(client);
                }

                return listClientViewModel;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al traer los clientes");
                throw new Exception("Error al traer los cliente, message: " + e.Message);
            }
        }

        public ClientViewModel GetClientViewModelById(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var client =  _clientRepository.GetClientById(id);
                if (client == null)
                {
                    //_logger.LogWarning("Error al traer los clientes");
                    throw new Exception("Error en la obtencion del cliente");
                }
                var clientViewModel = new ClientViewModel();
                _converterClientToClientViewModel.Map(client, clientViewModel);
                return clientViewModel;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("exception al traer el cliente especifico");
                throw new Exception("Error al traer el cliente especificado, message: " + e.Message);
            }
        }

        public ClientViewModel GetClientViewModelByIdentityGuid(string identityGuid)
        {
            var clientViewModel =   _clientRepository.GetClientByIdentityGuid(identityGuid);
            return _converterClientToClientViewModel.Map(clientViewModel);
        }

        public void UpateClient(int? id, ClientViewModel clientViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var client =  _clientRepository.GetClientById(id);
                if (client == null)
                {
                    //_logger.LogWarning("Error al traer el cliente");
                    throw new Exception("Error en la obtencion del cliente");
                }

                _converterClientViewModelToClient.Map(clientViewModel, client);
                 _clientRepository.UpdateClient(client);
                //_logger.LogInformation("Cliente Actualizado");
            }
            catch (Exception e)
            {
                //_logger.LogWarning("exception al actualizar el cliente especifico");
                throw new Exception("Error al actualizar el cliente. message: " + e.Message);
            }
        }
    }
}