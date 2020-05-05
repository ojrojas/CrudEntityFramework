using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.ServiceGyms
{
    /// <summary>
    /// Lógica de interacción para ServiceGymsDetails.xaml
    /// </summary>
    public partial class ServiceGymsDetails : UserControl
    {
        public ServiceGymsDetails(int serviceGymsId)
        {
            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            _serviceGymsId = serviceGymsId;
            GetServiceGym(_serviceGymsId);
            EventsSource();
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
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, typeof(ServiceGymsList), null);
        }

        private readonly IServiceGymServiceViewModel _serviceGymRepository;
        private readonly int? _serviceGymsId;

        private  void GetServiceGym(int? _serviceGymsId)
        {
            var serviceGym =  _serviceGymRepository.GetServiceGymByIdViewModel(_serviceGymsId);
            Type.Content = serviceGym.ServiceGymType.Type;
            Id.Text = serviceGym.Id.ToString();
            Name.Content = serviceGym.Name;
        }

        private void EditServiceGym_Click(object sender, RoutedEventArgs e)
        {

            //if (_serviceGymsId != null)
                //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsDetails, new ServiceGymsEdit((int)_serviceGymsId));
                //(Application.Current.MainWindow.FindName("KallpaBoxContent") as Frame).Content = new ServiceGymsEdit(int.Parse(_serviceGymsId.ToString()));
        }

        private void BackToListServiceGym_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, new ServiceGymsList());
            //(Application.Current.MainWindow.FindName("KallpaBoxContent") as Frame).Content = new ServiceGymsList();
        }
    }
}
