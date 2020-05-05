using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.ServiceGymTypes
{
    /// <summary>
    /// Lógica de interacción para ServiceGymTypesDetails.xaml
    /// </summary>
    public partial class ServiceGymTypesDetails : UserControl
    {
      
        public ServiceGymTypesDetails(int serviceGymTypeId)
        {
            _serviceGymTypeRepository = new ServiceGymTypeServiceViewModel();
            InitializeComponent();
            _serviceGymTypeId = serviceGymTypeId;
            GetServiceGymType(_serviceGymTypeId);
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
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeCreate, typeof(ServiceGymTypesList), null);
        }


        private readonly int? _serviceGymTypeId;
        private readonly IServiceGymTypeServiceViewModel _serviceGymTypeRepository;


        private void BackToListServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current.MainWindow.FindName("KallpaBoxContent") as Frame).Content = new ServiceGymTypesList();
        }

        private void EditServiceGymType_Click(object sender, RoutedEventArgs e)
        {

            //if (_serviceGymTypeId != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeEdit, new ServiceGymTypesEdit((int)_serviceGymTypeId));
        }

        private  void GetServiceGymType(int? serviceGymTypeId)
        {
            var serviceGymTypeViewModel =  _serviceGymTypeRepository.GetServiceGymTypeByIdViewModel(serviceGymTypeId);
            Type.Content = serviceGymTypeViewModel.Type;
        }
    }
}
