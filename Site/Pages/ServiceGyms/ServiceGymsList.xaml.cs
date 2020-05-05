using KallpaBox.Core.Entities;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Site.Pages.ServiceGyms
{
    /// <summary>
    /// Lógica de interacción para ServiceGymsList.xaml
    /// </summary>
    public partial class ServiceGymsList : UserControl
    {
        private readonly IServiceGymServiceViewModel _serviceGymRepository;
        private readonly IServiceGymTypeServiceViewModel _serviceGymTypeRepository;
        public ServiceGymsList()
        {
            InitializeComponent();
            _serviceGymRepository = new ServiceGymServiceViewModel();
            _serviceGymTypeRepository = new ServiceGymTypeServiceViewModel();
            this.Loaded += (o,e) =>this.GetDataGrid();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsEdit, new ServiceGymsEdit((int)(parameter)));
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsDetails, new ServiceGymsDetails((int)(parameter)));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var parameter = ((Button)sender).CommandParameter;
            //if (parameter != null)
            //    ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsDelete, new ServiceGymsDelete((int)(parameter)));
        }

        private void CreateServiceGym_Click(object sender, RoutedEventArgs e)
        {
                //ProcesarAbrirVentana.AbrirVentana(ConstantsServiceGym.NameWindowServiceGymsCreate, new ServiceGymsCreate());
        }

        private  void GetDataGrid()
        {
            var list = new List<ServiceGymViewModel>();
            var listServiceGyms =  _serviceGymRepository.GetAllServiceGymViewModel();
            foreach (var i in listServiceGyms)
            {
                var ServiceGymserviceGymType =  _serviceGymTypeRepository.GetServiceGymTypeByIdViewModel(i.ServiceGymTypeId);
                var serviceGymType = new ServiceGymType()
                {
                    Id = ServiceGymserviceGymType.Id,
                    Type = ServiceGymserviceGymType.Type
                };

                i.ServiceGymType = serviceGymType;
                list.Add(i);
            }

            DataGridServiceGyms.ItemsSource = list;
            this.DataGridServiceGyms.UpdateLayout();
        }
    }
}
