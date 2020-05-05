using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Site.Pages.ServiceGymContracts
{
    /// <summary>
    /// Lógica de interacción para ServiceGymContractsDetails.xaml
    /// </summary>
    public partial class ServiceGymContractsDetails : UserControl
    {
        public ServiceGymContractsDetails(int serviceGymContractId)
        {
            _serviceGymContractRepository = new ServiceGymContractServiceViewModel();
            InitializeComponent();
            _serviceGymContractId = serviceGymContractId;
            GetServiceGymContract(serviceGymContractId);
            this.EventsSource();
        }

        private readonly IServiceGymContractServiceViewModel _serviceGymContractRepository;
        private readonly int? _serviceGymContractId;

        /// <summary>
        /// Eventos de la ventana
        /// </summary>
        private void EventsSource()
        {
            this.Unloaded += (o, e) => CerrarVentana();
        }

        private void CerrarVentana()
        {
            //ProcesarAbrirVentana.CerrarVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsDetails);
        }

        private void BackToListServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, new ServiceGymContractsList());
        }

        private void EditServiceGymContract_Click(object sender, RoutedEventArgs e)
        {
            //if (_serviceGymContractId != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymContracts.NameWindowServiceGymContractsEdit, new ServiceGymContractsEdit((int)_serviceGymContractId));
        }

        private  void GetServiceGymContract(int? serviceGymContractId)
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
