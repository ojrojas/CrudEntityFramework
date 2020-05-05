using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;

namespace Site.Pages.ServiceGymContracts
{
    /// <summary>
    /// Lógica de interacción para ServiceGymContractsDelete.xaml
    /// </summary>
    public partial class ServiceGymContractsDelete : UserControl
    {
        public ServiceGymContractsDelete(int serviceGymContractId)
        {
            _serviceGymContractRepository = new ServiceGymContractServiceViewModel();
            InitializeComponent();
            _serviceGymContractId = serviceGymContractId;
            GetServiceGymContract(_serviceGymContractId);
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
            ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, typeof(ServiceGymContractsList), null);
        }

        private readonly IServiceGymContractServiceViewModel _serviceGymContractRepository;
        private readonly int _serviceGymContractId;


        private  void DeleteServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            var salir = MessageBox.Show("You want delete this item servicegym.", "KallpaBox", MessageBoxButton.YesNoCancel);
            if (salir == MessageBoxResult.Yes)
            {
                var serviceGymContractViewModel =  _serviceGymContractRepository.GetServiceGymContractViewViewById(_serviceGymContractId);
                try
                {
                    if (serviceGymContractViewModel != null)
                    {
                         _serviceGymContractRepository.DeleteServiceGymContractView(_serviceGymContractId, serviceGymContractViewModel);
                    }

                    //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, new ServiceGymContractsList());
                }
                catch (Exception ex)
                {

                    throw new Exception(string.Format(CultureInfo.CurrentCulture, $"Error : {ex.Message}"));
                }
            }
        }

        private void BackToListServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, new ServiceGymContractsList());
        }

        private  void GetServiceGymContract(int serviceGymContractId)
        {
            var serviceGymContract =  _serviceGymContractRepository.GetServiceGymContractViewViewById(serviceGymContractId);
            ServiceGym.Content = serviceGymContract.ServiceGym.Name;
            Id.Text = serviceGymContract.Id.ToString();
            Client.Content = serviceGymContract.Client.Identification +
                " " + serviceGymContract.Client.Name +
                " " + serviceGymContract.Client.LastName +
                " " + serviceGymContract.Client.SecondSurName;
            Price.Content = serviceGymContract.Price;
            Quantity.Content = serviceGymContract.Quantity;
            TypeQuantity.Content = serviceGymContract.TypeQuantity == (int)EnumsApp.TypeQuantitiesGym.Month ? "Months" : "Days";
            DateCelebrate.Content = serviceGymContract.DateCelebrate;
            DateExpiration.Content = serviceGymContract.DateExpiration;
        }
    }
}
