using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using System.Windows;
using System.Windows.Controls;

namespace Site.Pages.ServiceGymTypes
{
    /// <summary>
    /// Lógica de interacción para ServiceGymTypesList.xaml
    /// </summary>
    public partial class ServiceGymTypesList : UserControl
    {
        public ServiceGymTypesList()
        {
            _serviceGymTypeRepository = new ServiceGymTypeServiceViewModel();
            InitializeComponent();
            EnlazarEventos();
        }

        private void EnlazarEventos()
        {
            this.Loaded += (o,e) => this.GetDataServiceGymType();
        }

        private readonly IServiceGymTypeServiceViewModel _serviceGymTypeRepository;

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeEdit, new ServiceGymTypesEdit((int)(parameter)));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeDelete, new ServiceGymTypesEdit((int)(parameter)));
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeDetails, new ServiceGymTypesEdit((int)(parameter)));
        }

        private void CreateServiceGymType_Click(object sender, RoutedEventArgs e)
        {
                //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGymType.NameWindowServiceGymTypeCreate, new ServiceGymTypesCreate());
        }

        private  void GetDataServiceGymType()
        {
            var listServiceGymType =  _serviceGymTypeRepository.GetAllServiceGymTypeViewModel();
            DataGridServiceGymType.ItemsSource = listServiceGymType;
        }
    }
}
