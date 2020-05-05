using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Site.Pages.ServiceGymTypes
{
    /// <summary>
    /// Lógica de interacción para ServiceGymTypesCreate.xaml
    /// </summary>
    public partial class ServiceGymTypesCreate : UserControl
    {
       
        public ServiceGymTypesCreate()
        {
            _serviceGymTypeRepository = new ServiceGymTypeServiceViewModel();
            InitializeComponent();
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


        private readonly IServiceGymTypeServiceViewModel _serviceGymTypeRepository;

        private void BackToListServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeList, new ServiceGymTypesList());
        }

        private void CreateServiceGymType_Click(object sender, RoutedEventArgs e)
            {
            if (!InValidContext())
                return;
            var serviceGymTypeViewModel = new ServiceGymTypeViewModel()
            {
                Type = Type.Text,
            };

            try
            {
                _serviceGymTypeRepository.CreateServiceGymTypeViewModel(serviceGymTypeViewModel);
                CleanControls();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private bool InValidContext()
        {
            var valid = true;
            if (string.IsNullOrEmpty(Type.Text))
                valid = false;
            return valid;
        }

        private void CleanControls()
        {
            Type.Text = string.Empty;
        }
    }
}
