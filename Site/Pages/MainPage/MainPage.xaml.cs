using Site.Pages.Access;
using Site.Pages.Clients;
using Site.Pages.FingerPrints;
using Site.Pages.ServiceGymContracts;
using Site.Pages.ServiceGyms;
using Site.Pages.ServiceGymTypes;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Site.Pages.MainPage
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : UserControl
    {
        private bool _ignorarSeleccion;
        public MainPage()
        {
            InitializeComponent();
            NavigateToSelectedPage();
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            _ignorarSeleccion = true;
            ListaPaginas.SelectedValue = "/" + MainFrame.CurrentSource;
            _ignorarSeleccion = false;
        }

        private void NavigateToSelectedPage()
        {
            if (ListaPaginas.SelectedValue is Uri source)
            {
                MainFrame?.Navigate(source);
            }
        }

        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ignorarSeleccion)
            {
                NavigateToSelectedPage();
            }
        }


    }


    public class PagesList : List<ControlPageInfo>
    {
        public PagesList()
        {
            AddPage(typeof(ClientsList), "Lista Clientes");
            AddPage(typeof(ServiceGymsList), "Servicio de Gimnasio");
            AddPage(typeof(ServiceGymTypesList), "Tipo de Servicio Gimnasio");
            AddPage(typeof(ServiceGymContractsList), "Contratos de Servicios");
            AddPage(typeof(AccessPage), "Pagina de Acceso");
            AddPage(typeof(FingerPrintsCreate), "Crear Huellas Digitales");
        }

        private void AddPage(Type pageType, string displayName = null)
        {
            Add(new ControlPageInfo(pageType, displayName));
        }
    }

    public class ControlPageInfo
    {
        public ControlPageInfo(Type pageType, string displayName = null)
        {
            DisplayName = displayName ?? pageType.Name.Replace("Page", null);
            NavigateUri = new Uri($"{RuteDisplayPage(pageType.FullName)}.xaml", UriKind.Relative);
        }


        private string RuteDisplayPage(string rute)
        {
            var partidaruta = rute.Split('.');
            int contador = default(int);
            var direccionpantalla = string.Empty;
            foreach(var i in partidaruta)
            {
                if (contador != default(int))
                    direccionpantalla +=  "/" + i.ToString() ;
                contador++;
            }
            return direccionpantalla;
        }

        public string DisplayName { get; }

        public Uri NavigateUri { get; }

        //public override string ToString()
        //{
        //    return base.ToString();
        //}
    }
}
