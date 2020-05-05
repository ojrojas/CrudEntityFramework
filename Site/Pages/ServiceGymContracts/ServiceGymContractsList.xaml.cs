using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using KallpaBox.Core.Entities;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.ServiceGymContracts
{
    /// <summary>
    /// Lógica de interacción para ServiceGymContractsList.xaml
    /// </summary>
    public partial class ServiceGymContractsList : UserControl
    {
        public ServiceGymContractsList()
        {
            _serviceGymContractRepository = new ServiceGymContractServiceViewModel();
            _clientServiceRepository = new ClientServiceViewModel();
            _serviceGymsRepository = new ServiceGymServiceViewModel();
            InitializeComponent();
            this.Loaded += (o, e) => this.GetDataGrid();
        }

        private readonly IServiceGymContractServiceViewModel _serviceGymContractRepository;
        private readonly IClientServiceViewModel _clientServiceRepository;
        private readonly ServiceGymServiceViewModel _serviceGymsRepository ;

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsEdit, typeof(ServiceGymContractsEdit),int.Parse(parameter.ToString()));
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsDetails, typeof(ServiceGymContractsDetails),int.Parse(parameter.ToString()));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsDelete, typeof(ServiceGymContractsDelete),int.Parse(parameter.ToString()));
        }

        private  void GetDataGrid()
        {
            var list = new List<ServiceGymContractViewModel>();
            var listServiceGymContracts =  _serviceGymContractRepository.GetAllServiceGymContractViewView();
            foreach (var i in listServiceGymContracts)
            {
                var client =  _clientServiceRepository.GetClientViewModelById(i.ClientId);
                var serviceGym =  _serviceGymsRepository.GetServiceGymByIdViewModel(i.ServiceGymId);
                var serviceGymContractViewModel = new ServiceGymContractViewModel()
                {
                    Id = i.Id,
                    DateCelebrate = i.DateCelebrate,
                    Quantity = i.Quantity,
                    TypeQuantity = i.TypeQuantity,
                    Price = i.Price,
                    TypeQuantityText = i.TypeQuantity ==  (int)EnumsApp.TypeQuantitiesGym.Month ? "Months": "Days",
                    ServiceGym = new ServiceGym {
                        Id = serviceGym.Id,
                        Name = serviceGym.Name,
                        ServiceGymTypeId = serviceGym.ServiceGymTypeId,
                        ServiceGymType = serviceGym.ServiceGymType
                    },
                    Client = new Client{ Id = client.Id,
                        Identification = client.Identification,
                        Name = client.Name + " " + client.LastName + " " + client.SecondSurName
                    }
                };

                list.Add(serviceGymContractViewModel);
            }

            DataGridServiceGyms.ItemsSource = list;
        }

        private void CreateServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsCreate, typeof(ServiceGymContractsCreate),null);
        }
    }
}
