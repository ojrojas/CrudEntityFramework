using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.ServiceGyms
{
    /// <summary>
    /// Lógica de interacción para ServiceGymsCreate.xaml
    /// </summary>
    public partial class ServiceGymsCreate : UserControl
    {
        private readonly IServiceGymServiceViewModel _serviceGymRepository;
        public ServiceGymsCreate()
        {
            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            CleanControls();
            GetDataComboBox();
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

        private void BackToListServiceGym_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, new ServiceGymsList());
        }

        private void CreateServiceGym_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
                return;
            var serviceGymViewModel = new ServiceGymViewModel()
            {
                Name = Name.Text,
                ServiceGymTypeId = int.Parse(ServiceGymTypeId.SelectedValue.ToString())
            };

            try
            {
                _serviceGymRepository.CreateServiceGymViewModel(serviceGymViewModel);
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
            if (string.IsNullOrEmpty(Name.Text))
                valid = false;
            if (string.IsNullOrEmpty(ServiceGymTypeId.SelectedValue.ToString()))
                valid = false;
            return valid;
        }

        private  void GetDataComboBox()
        {
            var listServiceGym =  _serviceGymRepository.GetServiceGymTypes();
            ServiceGymTypeId.ItemsSource = listServiceGym;
            ServiceGymTypeId.DisplayMemberPath = "Type";
            ServiceGymTypeId.SelectedValuePath = "Id";
        }

        private void CleanControls()
        {
            ServiceGymTypeId.SelectedValue = null;
            Name.Text = string.Empty;
        }
    }
}
