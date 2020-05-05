using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Site.Pages.Clients
{
    /// <summary>
    /// Lógica de interacción para ClientsList.xaml
    /// </summary>
    public partial class ClientsList : UserControl
    {
        private readonly IClientServiceViewModel _clientRepository;
        public ObservableCollection<ClientViewModel> listaClientes = new ObservableCollection<ClientViewModel>();
        public ClientsList()
        {
            _clientRepository = new ClientServiceViewModel();
            InitializeComponent();
            this.DataGridClients.Items.Clear();
            this.DataBindingClients();
        }

        private void CreateClient_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsCreate, typeof( Clients),null);
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsEdit, typeof( ClientsEdit),parameter);
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsDetails, typeof (ClientsDetails),parameter);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            if (parameter != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsDelete, typeof(ClientsDelete),parameter);
        }

        private void DataBindingClients()
        {
            listaClientes.Clear();
            DataGridClients.ItemsSource = null;
            var list =  _clientRepository.GetAllClientViewModel();
            foreach (var i in list)
                listaClientes.Add(i);
            DataGridClients.ItemsSource = listaClientes;
            this.DataGridClients.UpdateLayout();
        }

        private  void ClientId_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(ClientId.Text))
                return;
            if (ClientId.Text != null || ClientId.Text.Contains("-"))
            {
                var clients =  _clientRepository.GetAllClientViewModel();
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
                            Name = i.Name,
                            MiddleName = i.MiddleName,
                            LastName = i.LastName,
                            SecondSurName = i.SecondSurName,
                            Identification = i.Identification,
                        });
                }

                DataGridClients.ItemsSource = new ObservableCollection<ClientViewModel>(listClients);
            }
        }
    }
}
