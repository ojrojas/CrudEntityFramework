using MaterialDesignThemes.Wpf;
using Site.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Site
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Snackbar Snackbar;
        private bool _ignoreSelectionChange;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel(MainSnackbar.MessageQueue);
            Snackbar = this.MainSnackbar;

            NavigateToSelectedPage();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //until we had a StaysOpen glag to Drawer, this will help with scroll bars
            var dependencyObject = Mouse.Captured as DependencyObject;
            while (dependencyObject != null)
            {
                if (dependencyObject is ScrollBar) return;
                dependencyObject = VisualTreeHelper.GetParent(dependencyObject);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void NavigateToSelectedPage()
        {
            if (ListaMenusKallpaBox.SelectedValue is Uri source)
            {
                ContenedorVentanas?.Navigate(source);
            }

            MenuToggleButton.IsChecked = false;
        }

        private void PagesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_ignoreSelectionChange)
            {
                NavigateToSelectedPage();
            }
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            _ignoreSelectionChange = true;
            ListaMenusKallpaBox.SelectedValue = ContenedorVentanas.CurrentSource;
            _ignoreSelectionChange = false;
        }

        private void SalirApp_Click(object sender, RoutedEventArgs e)
        {
            var salir = MessageBox.Show("Desea salir de Kallpabox App. ?", "Kallpabox Salir", MessageBoxButton.YesNoCancel);
            if (salir == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }
    }
}

