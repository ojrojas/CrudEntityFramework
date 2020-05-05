using System;
using System.Windows;
using System.Windows.Controls;

namespace Site.Utils
{
    public static class ProcesarAbrirVentana
    {
        public static void AbrirVentana(string nombreVentana, Type ventana, object parametro)
        {
            var instancia = default(object);
            var dataContext = Application.Current.MainWindow.FindName("ContenedorVentanas") as Frame;
            var _navigateUrl = new Uri($"{EstablecerRutasAbsolutas(ventana.FullName)}.xaml", UriKind.Relative);
            if (parametro != default(object))
            {
                instancia = Activator.CreateInstance(ventana, parametro);
                dataContext?.Navigate(instancia, parametro);
            }
            else
            {
                instancia = Activator.CreateInstance(ventana);
                dataContext?.Navigate(instancia);
            }
            dataContext.UpdateLayout();
        }

        private static string EstablecerRutasAbsolutas(string ruta)
        {
            var nuevaruta = ruta.Replace('.', '/');
            var rutaRetornada = nuevaruta.Replace("Site", "");
            return rutaRetornada;
        }

        //public static void CerrarVentana(string nombreVentana)
        //{
        //    var dataContext = Application.Current.MainWindow.FindName("ListaMenusKallpaBox") as ListBox;
        //    var items = dataContext.ItemsSource as IEnumerable<MenusItems>;
        //    var nuevosItems = items.ToList();
        //    if (nuevosItems.Any(x => x.Name == nombreVentana))
        //        nuevosItems.Remove(nuevosItems.Where(x => x.Name == nombreVentana).FirstOrDefault());
        //    dataContext.ItemsSource = nuevosItems;
        //    dataContext.UpdateLayout();
        //}
    }
}
