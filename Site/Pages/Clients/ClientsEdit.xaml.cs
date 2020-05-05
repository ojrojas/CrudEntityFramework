using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Microsoft.Win32;
using Site.Constants;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.Clients
{
    /// <summary>
    /// Lógica de interacción para ClientsEdit.xaml
    /// </summary>
    public partial class ClientsEdit : UserControl
    {
        private readonly IClientServiceViewModel _clientRepository;
        private readonly IPhotoService _photoRepository;
        private string _fileName;
        private readonly int _idClient;

        public ClientsEdit(int idClient)
        {
            _clientRepository = new ClientServiceViewModel();
            _photoRepository = new PhotoService();
            InitializeComponent();
            _idClient = idClient;
            this.Loaded += (e, o) => this.AbrirVentana(_idClient);
            this.EventsSource();
        }

        private void EventsSource()
        {
            //this.Unloaded += (o, e) => CerrarVentana();Oscar
            this.Age.PreviewTextInput += this.TextBoxNumeroEvent;
            this.Phone.PreviewTextInput += this.TextBoxNumeroEvent;
            this.Identification.PreviewTextInput += this.TextBoxNumeroEvent;
        }

        private void TextBoxNumeroEvent(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(ConstantsClients.RegexNumeros);
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CerrarVentana()
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsList, typeof(ClientsList), null);
        }

        private void AbrirVentana(int _idClient)
        {
            GetClientEdit(_idClient);
        }

        void NavigationService_LoadCompleted(object sender, NavigationEventArgs e)
        {
            bool test = (bool)e.ExtraData;
        }

        private void InsertImage_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "txt",
                Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg|GIF Files (*.gif)|*.gif",
                FilterIndex = 3,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == true)
            {
                _fileName = openFileDialog1.FileName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(_fileName);
                bitmap.EndInit();
                ImageClient.Source = bitmap;
                return;
            }

            ImageClient.Source = null;
        }

        private  void GetClientEdit(int id)
        {
            var client =  _clientRepository.GetClientViewModelById(id);


            Id.Text = client.Id.ToString();
            Name.Text = client.Name;
            MiddleName.Text = client.MiddleName;
            LastName.Text = client.LastName;
            SecondSurName.Text = client.SecondSurName;
            Age.Text = client.Age.ToString();
            Phone.Text = client.Phone;
            Address.Text = client.Address;
            Identification.Text = client.Identification;
            IdentityGuid.Text = client.IdentityGuid;

            var photo =  _photoRepository.GetPhotoClientId(client.Id);

            if (photo != null)
            {
                if (!string.IsNullOrWhiteSpace(photo.PhotoClient))
                        ImageClient.Source = BitmapFromUri.BitmapFromPath(new Uri(photo.PhotoClient));
            }
        }

        private  void EditClient_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
                return;
            var clientViewModel = new ClientViewModel()
            {
                Id = int.Parse(Id.Text),
                Name = Name.Text,
                MiddleName = MiddleName.Text,
                LastName = LastName.Text,
                SecondSurName = SecondSurName.Text,
                Age = int.Parse(Age.Text),
                Address = Address.Text,
                Identification = Identification.Text,
                IdentityGuid = IdentityGuid.Text,
                Phone = Phone.Text
            };

            try
            {
                 _clientRepository.UpateClient(clientViewModel.Id, clientViewModel);
                var rutaBorrar = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\KallpaBox\\TempAssets";
                var rutaDesktop = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\KallpaBox\\Assets";
                var photo =  _photoRepository.GetPhotoClientId(_idClient);
                var rutaTemp = rutaBorrar + "\\" + _idClient + ".jpg";
                if (ImageClient.Source != null)
                {

                    if (!(Directory.Exists(rutaDesktop)))
                    {
                        Directory.CreateDirectory(rutaDesktop);
                    }

                    if (!(Directory.Exists(rutaBorrar)))
                    {
                        Directory.CreateDirectory(rutaBorrar);
                    }

                    var imagetemp = ImageClient.Source as BitmapSource;

                    ImageClient.Source = null;
                    ImageClient.UpdateLayout();
                    File.Delete(photo.PhotoClient);
                    Thread.Sleep(1000);

                    if (photo != null)
                    {
                        photo.PhotoClient = rutaDesktop + "\\" + _idClient + ".jpg";
                         _photoRepository.UpdatePhoto(photo);
                    }
                    else
                    {
                        photo = new Photo()
                        {
                            ClientId = _idClient,
                            PhotoClient = rutaDesktop + "\\" + _idClient + ".jpg",
                            LengthPhoto = FileInfoExtension.ObtenerTamanoArchivo(photo.PhotoClient)
                        };

                         _photoRepository.CreatePhoto(photo);
                    }

                   
                    BitmapEncoder encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(imagetemp));

                    FileIOPermission f = new FileIOPermission(FileIOPermissionAccess.Write, rutaDesktop);
                    f.AllLocalFiles = FileIOPermissionAccess.Write;
                    try
                    {
                        f.Demand();
                    }
                    catch (SecurityException s)
                    {
                        Console.WriteLine(s.Message);
                    }

                    using (var fileStream = new FileStream(photo.PhotoClient, FileMode.Create))
                    {
                        encoder.Save(fileStream);
                    }
                }

                CleanControls();
                PresentationMessage.ShowMessegeQueue(this.SnackBarEditClients.MessageQueue, "Cliente editado con exito");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error {ex.Message}", ex);
            }
        }

        private void BackToListClients_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsList, typeof(ClientsList),null);
        }

        private bool InValidContext()
        {
            var valid = true;
            if (string.IsNullOrEmpty(Name.Text))
                valid = false;
            if (string.IsNullOrEmpty(LastName.Text))
                valid = false;
            if (string.IsNullOrEmpty(Phone.Text))
                valid = false;
            if (string.IsNullOrEmpty(Address.Text))
                valid = false;
            if (string.IsNullOrEmpty(Age.Text))
                valid = false;
            if (string.IsNullOrEmpty(Identification.Text))
                valid = false;
            return valid;
        }

        private void CleanControls()
        {
            Name.Text = string.Empty;
            MiddleName.Text = string.Empty;
            LastName.Text = string.Empty;
            SecondSurName.Text = string.Empty;
            Age.Text = string.Empty;
            Address.Text = string.Empty;
            Identification.Text = string.Empty;
            Phone.Text = string.Empty;
            ImageClient.Source = null;
        }
    }
}
