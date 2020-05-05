using System;
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
    /// Lógica de interacción para ClientsDetails.xaml
    /// </summary>
    public partial class ClientsDetails : UserControl
    {
        private readonly IClientServiceViewModel _clientRepository;
        private readonly IPhotoService _photoRepository;

        private readonly int? _clientId;
        public ClientsDetails(int idClient)
        {
            _clientRepository = new ClientServiceViewModel();
            _photoRepository = new PhotoService();
            InitializeComponent();
            GetClientDetails(idClient);
            _clientId = idClient;
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

        private  void GetClientDetails(int id)
        {
            var client =  _clientRepository.GetClientViewModelById(id);
            
            Name.Content= client.Name;
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

        private void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (_clientId != null)
                ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsEdit, typeof(ClientsEdit), _clientId);
        }
    }
}
