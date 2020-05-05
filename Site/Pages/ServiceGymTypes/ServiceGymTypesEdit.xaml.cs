using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.ServiceGymTypes
{
    /// <summary>
    /// Lógica de interacción para ServiceGymTypesEdit.xaml
    /// </summary>
    public partial class ServiceGymTypesEdit : UserControl
    {
        public ServiceGymTypesEdit(int serviceGymTypeId)
        {
            InitializeComponent();
            _serviceGymTypeRepository = new ServiceGymTypeServiceViewModel();
            GetServiceGymTypeEdit(serviceGymTypeId);
            _serviceGymTypeId = serviceGymTypeId;
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
        private readonly int _serviceGymTypeId;

        private void BackToListServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeList, typeof(ServiceGymTypesList),null);
        }

        private  void GetServiceGymTypeEdit(int id)
        {
            var serviceGymType=  _serviceGymTypeRepository.GetServiceGymTypeByIdViewModel(id);
            Type.Text = serviceGymType.Type;
            Id.Text = serviceGymType.Id.ToString();
        }

        private void EditServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
                return;
            var serviceGymTypeViewModel = new ServiceGymTypeViewModel()
            {
                Type = Type.Text,
                Id = int.Parse(Id.Text)
            };

            try
            {
                _serviceGymTypeRepository.UpateServiceGymTypeViewModel(_serviceGymTypeId, serviceGymTypeViewModel);
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
