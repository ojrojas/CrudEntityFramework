using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Futronic.SDKHelper;
using KallpaBox.Infraestructura.Sdks.Futronics;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.FingerPrints
{
    /// <summary>
    /// Lógica de interacción para FingerPrintsCreate.xaml
    /// </summary>
    public partial class FingerPrintsCreate : UserControl
    {
        public FingerPrintsCreate()
        {
            _clientRepository = new ClientServiceViewModel();
            m_bInitializationSuccess = false;
            InitializeComponent();
            GetDataClients();
            FutronicEnrollment dummy = new FutronicEnrollment();

            try
            {
                m_DatabaseDir = GetDatabaseDir();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Inicialización fallido. La aplicacion se podra cerrar.\nNo puedes crear el folder para huellas . Acceso denegado.", "KallpaBox Security",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            catch (IOException)
            {
                MessageBox.Show("Inicialización fallido. La aplicacion se podra cerrar.\nNo puedes crear el folder para huellas . Acceso denegado.", "KallpaBox Security",
                MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
        }

        private readonly IClientServiceViewModel _clientRepository;
        const string kCompanyName = "Futronic";
        const string kProductName = "SDK 4.0";
        const string kDbName = "DataBaseNet";
        private int _idClient;

        /// <summary>
        /// This delegate enables hronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// Delegado que setea el resutado en el snackbar
        /// </summary>
        /// <param name="text"></param>
        delegate void SetStatusMessageCallBack(string text);

        delegate void SetLimpiarControlComboCallBack();

        /// <summary>
        /// This delegate enables hronous calls for setting
        /// the text property on a identification limit control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetIdentificationLimitCallback(int limit);

        /// <summary>
        /// This delegate enables hronous calls for setting
        /// the Image property on a PictureBox control.
        /// </summary>
        /// <param name="hBitmap">the instance of Bitmap class</param>
        delegate void SetImageCallback(Bitmap hBitmap);

        /// <summary>
        /// This delegate enables hronous calls for setting
        /// the Enable property on a buttons.
        /// </summary>
        /// <param name="bEnable">true to enable buttons, otherwise to disable</param>
        delegate void EnableControlsCallback(bool bEnable);

        /// <summary>
        /// Contain reference for current operation object
        /// </summary>
        private FutronicSdkBase m_Operation;

        private bool m_bExit;

        /// <summary>
        /// The type of this parameter is depending from current operation. For
        /// enrollment operation this is DbRecord.
        /// </summary>
        private Object m_OperationObj;

        /// <summary>
        /// A directory name to write user's information.
        /// </summary>
        private string m_DatabaseDir;

#pragma warning disable CS0414 // El campo 'FingerPrintsCreate.m_bInitializationSuccess' está asignado pero su valor nunca se usa
        private bool m_bInitializationSuccess;
#pragma warning restore CS0414 // El campo 'FingerPrintsCreate.m_bInitializationSuccess' está asignado pero su valor nunca se usa

        private void InsertFingerPrintClient_Click(object sender, RoutedEventArgs e)
        {
            DbRecord User = new DbRecord();

            if (string.IsNullOrEmpty(Identificacion.Text))
            {
                MessageBox.Show("Debe ingresar un nombre de usuario.", "KallpaBox Security",
                                 MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // Try creat the file for user's information
            if (IsUserExists(Identificacion.Text))
            {
                MessageBoxResult nResponse;
                nResponse = MessageBox.Show("El usuario ya existe. ¿Quieres reemplazarlo?",
                                            "KallpaBox Security.",
                                            MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (nResponse == MessageBoxResult.No)
                    return;
            }
            else
            {
                try
                {
                    CreateFile(Identificacion.Text);
                }
                catch (DirectoryNotFoundException)
                {
                    MessageBox.Show("No se puede crear un archivo para guardar la información de un usuario.", "KallpaBox Security",
                                     MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                catch (IOException)
                {
                    MessageBox.Show(string.Format("Nombre de usuario incorrecto '{0}'.", Identificacion.Text), "KallpaBox Security"
                                    , MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
            }

            User.UserName = Identificacion.Text;

            m_OperationObj = User;
            FutronicSdkBase dummy = new FutronicEnrollment();
            if (m_Operation != null)
            {
                m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control properties
            m_Operation.FakeDetection = true;
            m_Operation.FFDControl = true;
            m_Operation.FARN = 116;
            m_Operation.Version = (VersionCompatible)2;
            m_Operation.FastMode = false;
            ((FutronicEnrollment)m_Operation).MIOTControlOff = true;
            ((FutronicEnrollment)m_Operation).MaxModels = 3;

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete += new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            // start enrollment process
            ((FutronicEnrollment)m_Operation).Enrollment();
        }


        private void OnEnrollmentComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                // set status string
                szMessage.Append("El proceso de inscripción finalizó con éxito. ");
                szMessage.Append($"Calidad de registro huella : {((FutronicEnrollment)m_Operation).Quality.ToString()} ");
                this.SetStatusText(szMessage.ToString());

                // Set template into user's information and save it
                DbRecord User = (DbRecord)m_OperationObj;
                User.Template = ((FutronicEnrollment)m_Operation).Template;

                String szFileName = Path.Combine(m_DatabaseDir, User.UserName);

                var client = _clientRepository.GetClientViewModelById(_idClient);
                client.IdentityGuid = User.UserName;

                var clientViewModel = new ClientViewModel()
                {
                    Id = client.Id,
                    Identification = client.Identification,
                    IdentityGuid = User.UserName,
                    Name = client.Name,
                    MiddleName = client.MiddleName,
                    LastName = client.LastName,
                    SecondSurName = client.SecondSurName,
                    Address = client.Address,
                    Age = client.Age,
                    Phone = client.Phone
                };

                _clientRepository.UpateClient(_idClient, clientViewModel);
                SetStatusMessageSnackBar("Huella registrado con exito");
                this.SetLimpiarCombo();

                if (!User.Save(szFileName))
                {
                    MessageBox.Show("No se puede guardar la información de los usuarios en el archivo." + szFileName,
                                     "KallpaBox Security.",
                                     MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            else
            {
                szMessage.Append("El proceso de inscripción falló ");
                szMessage.Append("Error descripción: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
                this.SetStatusText(szMessage.ToString());
            }

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete -= new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            m_OperationObj = null;
        }

        private void GetDataClients()
        {
            var clients = _clientRepository.GetAllClientViewModel();
            var listclients = new List<ClientViewModel>();
            foreach (var i in clients)
            {
                listclients.Add(new ClientViewModel()
                {
                    Id = i.Id,
                    Name = i.Identification + " - " + i.Name + " " + i.LastName + " " + i.SecondSurName
                });
            }
            ClientId.ItemsSource = null;
            ClientId.ItemsSource = listclients;
            ClientId.DisplayMemberPath = "Name";
            ClientId.SelectedValuePath = "Id";
        }

        private void SetIdentificationLimit(int nLimit)
        {
            if (this.IdentificationLimits.Dispatcher.Thread == Thread.CurrentThread)
            {
                if (nLimit == Int32.MaxValue)
                {
                    IdentificationLimits.Content = "Identification limit: No limits";
                }
                else
                {
                    IdentificationLimits.Content = String.Format("Identification limit: {0}", nLimit);
                }
            }
            else
            {
                SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
                Dispatcher.Invoke(d, new object[] { nLimit });
            }
        }

        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Pon el dedo en el dispositivo, por favor ...");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Quite el dedo del dispositivo, por favor...");
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (ImageClient.Dispatcher.Thread == Thread.CurrentThread)
            {
                MemoryStream ms = new MemoryStream();
                hBitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                ms.Seek(0, SeekOrigin.Begin);
                image.StreamSource = ms;
                image.EndInit();

                ImageClient.Source = image;
            }
            else
            {
                SetImageCallback d = new SetImageCallback(this.UpdateScreenImage);
                ImageClient.Dispatcher.Invoke(d, new object[] { hBitmap });
            }
        }

        private bool OnFakeSource(FTR_PROGRESS Progress)
        {
            if (m_bExit)
                return true;

            MessageBoxResult result;
            result = MessageBox.Show("Fuente o objeto falso detectado. o el huellero esta sucio.\n¿Quieres continuar el proceso?",
                                     "KallpaBox Security",
                                     MessageBoxButton.YesNo,
                                     MessageBoxImage.Question);
            return (result == MessageBoxResult.No);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Operation.OnCalcel();
        }

        private void ClientId_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetStatusText(string.Empty);
            this.SetLimpiarCombo();
            if (ClientId.SelectedValue != null)
                return;
            if (ClientId.Text != null || ClientId.Text.Contains("-"))
            {
                var clients = _clientRepository.GetAllClientViewModel();

                ClientId.ItemsSource = null;
                var listClients = new List<ClientViewModel>();
                var searchClients = (from i in clients
                                     where i.Name.ToLower().Contains(ClientId.Text.ToLower()) ||
                   i.LastName.ToLower().Contains(ClientId.Text.ToLower()) ||
                   i.Identification.ToLower().Contains(ClientId.Text.ToLower())
                                     select i);
                foreach (var i in searchClients)
                {
                    listClients.Add(
                        new ClientViewModel()
                        {
                            Id = i.Id,
                            Name = i.Identification + " - " + i.Name + " " + i.LastName + " " + i.SecondSurName
                        });
                }

                ClientId.ItemsSource = listClients;
                ClientId.DisplayMemberPath = "Name";
                ClientId.SelectedValuePath = "Id";
            }
        }

        private void SetStatusText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.lblMessage.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.lblMessage.Content = text;
                this.UpdateLayout();

            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetStatusText);
                lblMessage.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }

        private void SetLimpiarCombo()
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.ClientId.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.ClientId.ItemsSource= null;
                this.Identificacion.Text = string.Empty;
                this.UpdateLayout();
            }
            else
            {
                SetLimpiarControlComboCallBack d = new SetLimpiarControlComboCallBack(this.SetLimpiarCombo);
                ClientId.Dispatcher.BeginInvoke(d, new object[] {  });
            }
        }

        private void SetStatusMessageSnackBar(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.SnackBarFingerPrints.Dispatcher.Thread == Thread.CurrentThread)
            {
                PresentationMessage.ShowMessegeQueue(this.SnackBarFingerPrints.MessageQueue, text);
            }
            else
            {
                SetStatusMessageCallBack d = new SetStatusMessageCallBack(this.SetStatusMessageSnackBar);
                SnackBarFingerPrints.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }

        protected bool IsUserExists(String UserName)
        {
            String szFileName;
            szFileName = Path.Combine(m_DatabaseDir, UserName);
            return File.Exists(szFileName);
        }

        protected void CreateFile(String UserName)
        {
            string szFileName;
            szFileName = Path.Combine(m_DatabaseDir, UserName);
            File.Create(szFileName).Close();
            File.Delete(szFileName);
        }

        private void ClientId_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ClientId.SelectedValue != null)
            {
                var client = _clientRepository.GetClientViewModelById((int)ClientId.SelectedValue);
                if (!string.IsNullOrEmpty(client.IdentityGuid))
                {
                    Identificacion.Text = client.IdentityGuid;
                    _idClient = client.Id;
                }
                else
                {
                    Identificacion.Text = Guid.NewGuid().ToString();
                    _idClient = client.Id;
                }
            }
        }

        public static string GetDatabaseDir()
        {
            string szDbDir;
            szDbDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments, Environment.SpecialFolderOption.Create);
            szDbDir = Path.Combine(szDbDir, kCompanyName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }
            szDbDir = Path.Combine(szDbDir, kProductName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }

            szDbDir = Path.Combine(szDbDir, kDbName);
            if (!Directory.Exists(szDbDir))
            {
                Directory.CreateDirectory(szDbDir);
            }

            return szDbDir;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            m_bExit = true;
            if (m_Operation != null)
            {
                m_Operation.Dispose();
            }
        }
    }
}

