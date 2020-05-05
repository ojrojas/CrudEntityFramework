using System;
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
    /// Lógica de interacción para ServiceGymsEdit.xaml
    /// </summary>
    public partial class ServiceGymsEdit : UserControl
    {
        public ServiceGymsEdit(int serviceGymsId)
        {
            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            CleanControls();
            _serviceGymsId = serviceGymsId;
            GetDataComboBox();
            GetDataServiceGym(_serviceGymsId);
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
        private readonly int _serviceGymsId;

        public void EditServiceGym_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
                return;
            var serviceGymViewModel = new ServiceGymViewModel()
            {
                Id= int.Parse(Id.Text),
                Name = Name.Text,
                ServiceGymTypeId = int.Parse(ServiceGymTypeId.SelectedValue.ToString()),
            };

            try
            {
                _serviceGymRepository.UpateServiceGymViewModel(_serviceGymsId, serviceGymViewModel);
                CleanControls();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BackToListServiceGym_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, new ServiceGymsList());
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

        private  void GetDataServiceGym(int serviceGymId)
        {
            var serviceGym =  _serviceGymRepository.GetServiceGymByIdViewModel(serviceGymId);
            if(serviceGym != null)
            {
                ServiceGymTypeId.SelectedValue = serviceGym.ServiceGymTypeId;
                Name.Text = serviceGym.Name;
                Id.Text = serviceGym.Id.ToString();
            }
        }

        private void CleanControls()
        {
            Name.Text = string.Empty;
            Id.Text = string.Empty;
            ServiceGymTypeId.SelectedValue = null;
        }
    }
}
