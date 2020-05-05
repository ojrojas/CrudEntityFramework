using System;
using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.ServiceGymTypes
{
    /// <summary>
    /// Lógica de interacción para ServiceGymTypesDelete.xaml
    /// </summary>
    public partial class ServiceGymTypesDelete : UserControl
    {
        public ServiceGymTypesDelete(int serviceGymTypeId)
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

        private readonly int _serviceGymTypeId;
        private readonly IServiceGymTypeServiceViewModel _serviceGymTypeRepository;

        private  void DeleteServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            var salir = MessageBox.Show("You want delete this item servicegym type.", "KallpaBox", MessageBoxButton.YesNoCancel);
            if (salir == MessageBoxResult.Yes)
            {
                var serviceGymTypeViewModel =  _serviceGymTypeRepository.GetServiceGymTypeByIdViewModel(_serviceGymTypeId);

                try
                {
                    if (serviceGymTypeViewModel != null)
                    {
                         _serviceGymTypeRepository.DeleteServiceGymTypeViewModel(serviceGymTypeViewModel.Id, serviceGymTypeViewModel);
                    }

                    (Application.Current.MainWindow.FindName("KallpaBoxContent") as Frame).Content = new ServiceGymTypesList();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }

        private void BackToListServiceGymType_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeList, new ServiceGymTypesList());
        }

        private  void GetServiceGymType(int serviceGymTypeId)
        {
            var serviceGymTypeViewModel =  _serviceGymTypeRepository.GetServiceGymTypeByIdViewModel(serviceGymTypeId);
            Type.Content = serviceGymTypeViewModel.Type;
            Id.Text = serviceGymTypeViewModel.Id.ToString();
        }
    }
}
