using System;
using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.ServiceGyms
{
    /// <summary>
    /// Lógica de interacción para ServiceGymsDelete.xaml
    /// </summary>
    public partial class ServiceGymsDelete : UserControl
    {
        public ServiceGymsDelete(int serviceGymsId)
        {
            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            _serviceGymsId = serviceGymsId;
            GetServiceGym(serviceGymsId);
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

        private  void DeleteServiceGym_Click(object sender, RoutedEventArgs e)
        {
            var salir = MessageBox.Show("You want delete this item servicegym.", "KallpaBox", MessageBoxButton.YesNoCancel);
            if (salir == MessageBoxResult.Yes)
            {
                var serviceGymViewModel =  _serviceGymRepository.GetServiceGymByIdViewModel(_serviceGymsId);

                try
                {
                    if (serviceGymViewModel != null)
                    {
                         _serviceGymRepository.DeleteServiceGymViewModel(_serviceGymsId, serviceGymViewModel);
                    }

                    //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, new ServiceGymsList());

                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}",ex);
                }
            }
        }


        private  void GetServiceGym(int _serviceGymsId)
        {
            var serviceGym =  _serviceGymRepository.GetServiceGymByIdViewModel(_serviceGymsId);
            Type.Content = serviceGym.ServiceGymType.Type;
            Id.Text = serviceGym.Id.ToString();
            Name.Content = serviceGym.Name;
        }

        private void BackToListServiceGym_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsList, new ServiceGymsList());
        }
    }
}
