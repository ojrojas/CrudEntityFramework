using System;
using System.IO;
using System.Security;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
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
    /// Lógica de interacción para Clients.xaml
    /// </summary>
    public partial class Clients : UserControl
    {
        private readonly IClientServiceViewModel _clientRepository;
        private string _fileName;
        private readonly IPhotoService _photoRepository;

        public Clients()
        {
            InitializeComponent();
            _clientRepository = new ClientServiceViewModel();
            _photoRepository = new PhotoService();
            CleanControls();
            _fileName = string.Empty;
            this.EventsSource();
        }

        private void EventsSource()
        {
            //this.Unloaded += (o, e) => CerrarVentana();
            this.Age.PreviewTextInput += this.TextBoxNumeroEvent;
            this.Phone.PreviewTextInput += this.TextBoxNumeroEvent;
            this.Identification.PreviewTextInput += this.TextBoxNumeroEvent;
        }

        private void CerrarVentana()
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsCreate, typeof(ClientsList), null);
        }

        private void TextBoxNumeroEvent(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(ConstantsClients.RegexNumeros);
            e.Handled = regex.IsMatch(e.Text);
        }


        private void InsertImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Buscar imagenes.",

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
            }
        }

        private  void InsertClient_Click(object sender, RoutedEventArgs e)
        {
            if (!InValidContext())
            {
                PresentationMessage.ShowMessegeQueue(this.SnackBarClients.MessageQueue, "Por favor llene todos los campos para crear el cliente.");
                return;
            }
            var clientViewModel = new ClientViewModel()
            {
                Name = Name.Text,
                MiddleName = MiddleName.Text,
                LastName = LastName.Text,
                SecondSurName = SecondSurName.Text,
                Age = int.Parse(Age.Text),
                Address = Address.Text,
                Identification = Identification.Text,
                Phone = Phone.Text
            };

            try
            {
                var rutaDesktop = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\KallpaBox\\Assets";
                var idClient =  _clientRepository.CreateClient(clientViewModel);
                PresentationMessage.ShowMessegeQueue(this.SnackBarClients.MessageQueue, "Cliente creado con exito");

                var photo = new Photo()
                {
                    ClientId = idClient,
                    PhotoClient = rutaDesktop + "\\" + idClient + ".jpg",
                };

                if (ImageClient.Source != null)
                {
                    if (!(Directory.Exists(rutaDesktop)))
                    {
                        Directory.CreateDirectory(rutaDesktop);
                    }

                    if (Directory.Exists(rutaDesktop))
                    {
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.UriSource = new Uri(_fileName);
                        bitmap.EndInit();
                        BitmapEncoder encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(bitmap));

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

                        photo.LengthPhoto = FileInfoExtension.ObtenerTamanoArchivo(photo.PhotoClient);

                         _photoRepository.CreatePhoto(photo);
                    }
                }

                CleanControls();
            }
            catch (Exception ex)
            {
                throw new Exception("Error en aplicativo contacte con el administrador. {0}", ex);
            }
        }

        private void BackToListClients_Click(object sender, RoutedEventArgs e)
        {
            ProcesarAbrirVentana.AbrirVentana(ConstantsClients.NameWindowClientsList, typeof( ClientsList), null);
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
            _fileName = string.Empty;
        }
    }
}
