using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using KallpaBox.Core.Interfaces;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.Clients
{
    /// <summary>
    /// Lógica de interacción para ClientsDelete.xaml
    /// </summary>
    public partial class ClientsDelete : UserControl
    {
        private readonly IClientServiceViewModel _clientRepository;
        private readonly ISessionGymServiceViewModel _sessionRepository;
        private readonly IPhotoService _photoRepository;

        private readonly int? _clientId;

        public ClientsDelete(int clientId)
        {
            _clientRepository = new ClientServiceViewModel();
            _sessionRepository = new SessionGymServiceViewModel();
            _photoRepository = new PhotoService();
            InitializeComponent();
            _clientId = clientId;
            GetClientDelete(_clientId);
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
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsDelete, typeof(ClientsList), null);
        }

        private  void GetClientDelete(int? id)
        {
            var client =  _clientRepository.GetClientViewModelById(id);

            Name.Content = client.Name;
            MiddleName.Content = client.MiddleName;
            LastName.Content = client.LastName;
            SecondSurName.Content = client.SecondSurName;
            Age.Content = client.Age.ToString();
            Address.Content = client.Address;
            Identification.Content = client.Identification;
            Phone.Content = client.Phone;

            var photo =  _photoRepository.GetPhotoClientId(client.Id);

            if (photo != null)
            {
                if (!string.IsNullOrWhiteSpace(photo.PhotoClient))
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(photo.PhotoClient);
                    bitmap.EndInit();
                    ImageClient.Source = bitmap;
                }
            }
        }

        private void BackToListClients_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsList, typeof(ClientsList),null);
        }

        private  void DeleteClient_Click(object sender, RoutedEventArgs e)
        {
            var salir = MessageBox.Show("Deseas eliminar el registro.", "KallpaBox", MessageBoxButton.YesNoCancel);
            if (salir == MessageBoxResult.Yes)
            {
                var clientViewModel =  _clientRepository.GetClientViewModelById(_clientId);
                var sessions =  _sessionRepository.GetAllSessionGymViewByClientIdView(_clientId);

                try
                {
                    if (clientViewModel != null)
                    {
                         _clientRepository.DeleteClient(clientViewModel.Id, clientViewModel);
                    }
                    if (sessions.Any())
                    {
                        foreach (var i in sessions)
                        {
                             _sessionRepository.DeleteSessionGymView(i.ClientId);
                        }
                    }

                    PresentationMessage.ShowMessegeQueue(this.SnackBarDeleteClients.MessageQueue, "Cliente Eliminado con exito");


                    this.EliminarButton.IsEnabled = false;

                    Thread.Sleep(5000);

                    ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsList,typeof(ClientsList),null);
                }
                catch (Exception ex)
                {

                    throw new Exception(string.Format(CultureInfo.CurrentCulture,"Error: {0})",ex.Message));
                }
            }
        }
    }
}
