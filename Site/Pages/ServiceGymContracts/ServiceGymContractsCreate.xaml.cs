using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Site.Pages.ServiceGymContracts
{
    /// <summary>
    /// Lógica de interacción para ServiceGymContractsCreate.xaml
    /// </summary>
    public partial class ServiceGymContractsCreate : UserControl
    {
        public ServiceGymContractsCreate()
        {

            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            _serviceGymContractRepository = new ServiceGymContractServiceViewModel();
            _clientRepository = new ClientServiceViewModel();
            CleanControls();
            GetDataServiceGym();
            this.EventsSource();
        }

        /// <summary>
        /// Eventos de la ventana
        /// </summary>
        private void EventsSource()
        {
            this.Unloaded += (o, e) => CerrarVentana();
        }

        private void CerrarVentana()
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, typeof(ServiceGymContractsList), null);
        }

        private readonly IServiceGymServiceViewModel _serviceGymRepository;
        private readonly IClientServiceViewModel _clientRepository;
        private readonly IServiceGymContractServiceViewModel _serviceGymContractRepository;

        private void BackToListServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList,typeof(ServiceGymContractsList),null);
        }

        private  void CreateServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
                return;
            var serviceGymContractViewModel = new ServiceGymContractViewModel()
            {
                ServiceGymId = int.Parse(ServiceGymId.SelectedValue.ToString()),
                ClientId = int.Parse(ClientId.SelectedValue.ToString()),
                Price = decimal.Parse(Price.Text),
                Quantity = int.Parse(Quantity.Text),
                TypeQuantity = int.Parse(TypeQuantity.SelectedValue.ToString()),
                State = true
            };

            try
            {
                if(serviceGymContractViewModel.TypeQuantity == (int)EnumsApp.TypeQuantitiesGym.Month)
                {
                    serviceGymContractViewModel.DateExpiration = serviceGymContractViewModel.DateCelebrate.AddMonths(serviceGymContractViewModel.Quantity);
                }

                if (serviceGymContractViewModel.TypeQuantity == (int)EnumsApp.TypeQuantitiesGym.Days)
                {
                    serviceGymContractViewModel.DateExpiration = serviceGymContractViewModel.DateCelebrate.AddDays(serviceGymContractViewModel.Quantity);
                }
                 _serviceGymContractRepository.CreateServiceGymContract(serviceGymContractViewModel);
                ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, typeof(ServiceGymContractsList), null);

                CleanControls();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private  void GetDataServiceGym()
        {
            var listServiceGym =  _serviceGymRepository.GetAllServiceGymViewModel();
            ServiceGymId.ItemsSource = listServiceGym;
            ServiceGymId.DisplayMemberPath = "Name";
            ServiceGymId.SelectedValuePath = "Id";
            var clients =  _clientRepository.GetAllClientViewModel();
            var listclients = new List<ClientViewModel>();
            foreach (var i in clients)
            {
                listclients.Add(new ClientViewModel()
                {
                    Id = i.Id,
                    Name = i.Identification + " - " + i.Name + " " + i.LastName + " " + i.SecondSurName
                });
            }
            ClientId.ItemsSource = null;
            ClientId.ItemsSource = listclients;
            ClientId.DisplayMemberPath = "Name";
            ClientId.SelectedValuePath = "Id";
        }

        private bool InValidContext()
        {
            var valid = true;

            if (string.IsNullOrEmpty(Quantity.Text))
                valid = false;
            if (string.IsNullOrEmpty(TypeQuantity.Text))
                valid = false;
            if (string.IsNullOrEmpty(Price.Text))
                valid = false;
            if (string.IsNullOrEmpty(ServiceGymId.SelectedValue.ToString()))
                valid = false;
            if (string.IsNullOrEmpty(ClientId.SelectedValue.ToString()))
                valid = false;
            return valid;
        }

        private void CleanControls()
        {
            ServiceGymId.SelectedValue = null;
            ClientId.SelectedValue = null;
            Price.Text = string.Empty;
            Quantity.Text= string.Empty;
            TypeQuantity.SelectedValue = null;
        }

        private  void ClientId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ClientId.SelectedValue != null)
                return;
            if (ClientId.Text != null || ClientId.Text.Contains("-"))
            {
                var clients =  _clientRepository.GetAllClientViewModel();
                ClientId.ItemsSource = null;
                var listClients = new List<ClientViewModel>();
                var searchClients = (from i in clients
                                     where i.Name.ToLower().Contains(ClientId.Text.ToLower()) ||
                   i.LastName.ToLower().Contains(ClientId.Text.ToLower()) ||
                   i.Identification.ToLower().Contains(ClientId.Text.ToLower())
                                     select i);
                foreach (var i in searchClients)
                {
                    listClients.Add(
                        new ClientViewModel()
                        {
                            Id = i.Id,
                            Name = i.Identification + " - " + i.Name + " " + i.LastName + " " + i.SecondSurName
                        });
                }

                ClientId.ItemsSource = listClients;
                ClientId.DisplayMemberPath = "Name";
                ClientId.SelectedValuePath = "Id";
            }
        }

    }
}
