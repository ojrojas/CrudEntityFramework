using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using Site.Utils;

namespace Site.ViewModels
{
    public  class MenusItems : INotifyPropertyChanged
    {
        private string _name;
        private Type _content;
        private Uri _navigateUrl;
        private ScrollBarVisibility _horizontalScrollBarVisibilityRequirement;
        private ScrollBarVisibility _verticalScrollBarVisibilityRequirement;
        private Thickness _marginRequirement = new Thickness(16);

        public MenusItems(string name, Type content)
        {
            _name = name;
            Content = content;
            _navigateUrl = new Uri($"{EstablecerRutasAbsolutas(Content.FullName)}.xaml", UriKind.Relative);
        }

        public string Name
        {
            get { return _name; }
            set { this.MutateVerbose(ref _name, value, RaisePropertyChanged()); }
        }

        public Type Content
        {
            get { return _content; }
            set { this.MutateVerbose(ref _content, value, RaisePropertyChanged()); }
        }

        public Uri NavegateUrl
        {
            get { return _navigateUrl; }
            set { this.MutateVerbose(ref _navigateUrl, value, RaisePropertyChanged()); }
        }

        private string EstablecerRutasAbsolutas(string ruta)
        {
           var nuevaruta =  ruta.Replace('.', '/');
            var rutaRetornada = nuevaruta.Replace("Site", "");
            return rutaRetornada;
        }

        public ScrollBarVisibility HorizontalScrollBarVisibilityRequirement
        {
            get { return _horizontalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _horizontalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public ScrollBarVisibility VerticalScrollBarVisibilityRequirement
        {
            get { return _verticalScrollBarVisibilityRequirement; }
            set { this.MutateVerbose(ref _verticalScrollBarVisibilityRequirement, value, RaisePropertyChanged()); }
        }

        public Thickness MarginRequirement
        {
            get { return _marginRequirement; }
            set { this.MutateVerbose(ref _marginRequirement, value, RaisePropertyChanged()); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private Action<PropertyChangedEventArgs> RaisePropertyChanged()
        {
            return args => PropertyChanged?.Invoke(this, args);
        }
    }
}
