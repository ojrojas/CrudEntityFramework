
using Boxed.Mapping;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.Converts;
using Site.Interfaces;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Site.Services
{
    public class SessionGymServiceViewModel : ISessionGymServiceViewModel
    {
        private readonly ISessionGymService _sessionGymRepository;
        private readonly IMapper<SessionGymViewModel, SessionGym> _convertSessionGymViewModelToSessionGym ;
        private readonly IMapper<SessionGym, SessionGymViewModel> _convertSessionGymToSessionGymViewModel;


        public SessionGymServiceViewModel()
        {
            _sessionGymRepository = new SessionGymService();
            _convertSessionGymViewModelToSessionGym   = new SessionGymViewModelToSessionGym();
            _convertSessionGymToSessionGymViewModel = new SessionGymToSessionGymViewModel();
        }

        public  void CreateSessionGym(SessionGymViewModel sessionGymViewModel)
        {
            try
            {
                if (sessionGymViewModel != null)
                {
                    var sessionGym = _convertSessionGymViewModelToSessionGym.Map(sessionGymViewModel);
                     _sessionGymRepository.CreateSessionGym(sessionGym);
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

       
        public  void DeleteSessionGymView(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymType =  _sessionGymRepository.GetSessionGymById(id);
                if (serviceGymType == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                 _sessionGymRepository.DeleteSessionGym(id);
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

        public  IReadOnlyList<SessionGymViewModel> GetAllSessionGymViewByClientIdView(int? clientId)
        {
            try
            {
                var listSessionGymViewModel = new List<SessionGymViewModel>();
                var list =  _sessionGymRepository.GetSessionsGymByClientId(clientId);
                list.AsEnumerable();
                foreach (var i in list)
                {
                    var serviceGymType = _convertSessionGymToSessionGymViewModel.Map(i);
                    listSessionGymViewModel.Add(serviceGymType);
                }

                return listSessionGymViewModel;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("error al traer la lista de servicios, message:", e.Message);
                throw new Exception("Error al traer la lista de servicegymType, message:" + e.Message);
            }
        }

        public IReadOnlyList<SessionGymViewModel> GetAllSessionGymViewView()
        {
            try
            {
                var listSessionGymViewModel = new List<SessionGymViewModel>();
                var list =  _sessionGymRepository.ListAllSessionGym();
                list.AsEnumerable();
                foreach (var i in list)
                {
                    var serviceGymType = _convertSessionGymToSessionGymViewModel.Map(i);
                    listSessionGymViewModel.Add(serviceGymType);
                }

                return listSessionGymViewModel;
            }
            catch (Exception e)
            {
                //_logger.LogWarning("error al traer la lista de servicios, message:", e.Message);
                throw new Exception("Error al traer la lista de servicegymType, message:" + e.Message);
            }
        }

        public SessionGymViewModel GetSessionGymViewViewById(int? id)
        {
            try
            {
                if (id == null)
                {
                    throw new ArgumentNullException("El parametro id esta vacio");
                }
                var serviceGymType =  _sessionGymRepository.GetSessionGymById(id);
                if (serviceGymType == null)
                {
                    throw new ArgumentNullException("La entidad no puede ser nula");
                }

                return _convertSessionGymToSessionGymViewModel.Map(serviceGymType);

            }
            catch (Exception e)
            {
                //_logger.LogWarning("Error al devolver el serviceGymType, message:" + e.Message);
                throw new Exception("Error al devolver el serviceGymType, message:" + e.Message);
            }

        }

        public  void UpateSessionGymView(int? id, SessionGymViewModel sessionGymViewModel)
        {
            try
            {
                if (id == null)
                {
                    throw new Exception("El parametro id es un nulo");
                }
                var sessionGym =  _sessionGymRepository.GetSessionGymById(id);
                if (sessionGym == null)
                {
                    //_logger.LogWarning("Error al traer el service Gym");
                    throw new Exception("Error en la obtencion del service Gym");
                }

                _convertSessionGymViewModelToSessionGym.Map(sessionGymViewModel, sessionGym);
                 _sessionGymRepository.UpdateSessionGym(id, sessionGym);
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