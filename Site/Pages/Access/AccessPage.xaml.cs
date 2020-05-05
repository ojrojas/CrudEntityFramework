using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Futronic.SDKHelper;
using KallpaBox.Infraestructura.Sdks.Futronics;
using Site.Interfaces;
using Site.Services;
using Site.Utils;
using Site.ViewModels;

namespace Site.Pages.Access
{
    /// <summary>
    /// Lógica de interacción para AccessPage.xaml
    /// </summary>
    public partial class AccessPage : UserControl
    {
        public AccessPage()
        {
            _clientRepository = new ClientServiceViewModel();
            _serviceGymContractRepository = new ServiceGymContractServiceViewModel();
            m_bInitializationSuccess = false;
            InitializeComponent();
            FutronicEnrollment dummy = new FutronicEnrollment();

            try
            {
                m_DatabaseDir = GetDatabaseDir();
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("Inicialización falló. La aplicación estará cerrada. \nNo se puede crear una carpeta de base de datos. Acceso denegado.", "KallpaBox Security",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            catch (IOException)
            {
                MessageBox.Show("Inicialización falló. La aplicación estará cerrada. \nNo se puede crear una carpeta de base de datos. Acceso denegado.", "KallpaBox Security",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

           this.Loaded += (o,e)=> IdentityApp();
        }

        private readonly IClientServiceViewModel _clientRepository;
        const string kCompanyName = "Futronic";
        const string kProductName = "SDK 4.0";
        const string kDbName = "DataBaseNet";
        private readonly IServiceGymContractServiceViewModel _serviceGymContractRepository;

        /// <summary>
        /// This delegate enables hronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

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

#pragma warning disable CS0414 // El campo 'AccessPage.m_bInitializationSuccess' está asignado pero su valor nunca se usa
        private bool m_bInitializationSuccess;
#pragma warning restore CS0414 // El campo 'AccessPage.m_bInitializationSuccess' está asignado pero su valor nunca se usa


        private  void OnGetBaseTemplateComplete(bool bSuccess, int nRetCode)
        {
            SetIdentificationText("");
            SetNameText("");
            SetMiddleNameText("");
            SetLastNameText("");
            SetSecondSurNameText("");
            SetStatusClientRedText("");
            SetCelebrateContract("");
            SetExpirationContract("");

            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                this.SetStatusText("Identificación de inicio...");
                List<DbRecord> Users = (List<DbRecord>)m_OperationObj;

                int iRecords = 0;
                int nResult;
                FtrIdentifyRecord[] rgRecords = new FtrIdentifyRecord[Users.Count];
                foreach (DbRecord item in Users)
                {
                    rgRecords[iRecords] = item.GetRecord();
                    iRecords++;
                }
                nResult = ((FutronicIdentification)m_Operation).Identification(rgRecords, ref iRecords);
                if (nResult == FutronicSdkBase.RETCODE_OK)
                {
                    szMessage.Append("Proceso de identificación completo. Usuario: ");
                    if (iRecords != -1)
                    {
                        szMessage.Append(Users[iRecords].UserName);

                       if(!string.IsNullOrEmpty(Users[iRecords].UserName))
                        {
                            var clientViewModel =  _clientRepository.GetClientViewModelByIdentityGuid(Users[iRecords].UserName);
                            var contract =  _serviceGymContractRepository.GetServiceGymContractServiceViewModelByClientId(clientViewModel.Id); ;

                            if(contract == null)
                            {
                                SetMessageQueueSnackBar("Cliente no tiene un contrato registrado.");
                                ((FutronicIdentification)m_Operation).GetBaseTemplate();
                                return;
                            }

                            if (clientViewModel != null)
                            {
                                SetIdentificationText(clientViewModel.Identification);
                                SetNameText(clientViewModel.Name);
                                SetMiddleNameText(clientViewModel.MiddleName);
                                SetLastNameText(clientViewModel.LastName);
                                SetSecondSurNameText(clientViewModel.SecondSurName);
                                SetCelebrateContract(contract.DateCelebrate.ToString());
                                SetExpirationContract(contract.DateExpiration.ToString());

                                if (contract != null)
                                {
                                    if (contract.DateExpiration < DateTime.Now)
                                    {
                                        SetStatusClientGreenText("Cliente verificado");
                                        SetMessageQueueSnackBar("Cliente Verificado pero con contrato vencido");
                                        var contractUpdate = new ServiceGymContractViewModel()
                                        {
                                            State = false,
                                            Client = contract.Client,
                                            ClientId = contract.ClientId,
                                            DateExpiration = contract.DateExpiration,
                                            DateCelebrate = contract.DateCelebrate,
                                            Quantity = contract.Quantity,
                                            TypeQuantity = contract.TypeQuantity,
                                            Price = contract.Price,
                                            ServiceGym = contract.ServiceGym,
                                            ServiceGymId = contract.ServiceGymId,
                                            Id = contract.Id
                                        };
                                         _serviceGymContractRepository.UpateServiceGymContractView(contract.Id, contractUpdate);
                                    }
                                    else
                                    {
                                        SetStatusClientGreenText("Cliente verificado Bienvenido.");
                                        SetMessageQueueSnackBar("Cliente verificado y con contrato vigente");
                                    }

                                }
                                else
                                {
                                    SetStatusClientRedText("Tu tienes un contrato ??? ");
                                    SetMessageQueueSnackBar("El cliente no presenta contrato");
                                }
                            }
                            else
                            {
                                SetIdentificationText("");
                                SetNameText("");
                                SetMiddleNameText("");
                                SetLastNameText("");
                                SetSecondSurNameText("");
                                SetCelebrateContract("");
                                SetExpirationContract("");
                                SetStatusClientRedText("Tu tienes un contrato ??? ");
                                SetMessageQueueSnackBar("Cliente no tiene contrato.");
                            }
                        }
                       else
                        {
                            SetIdentificationText("");
                            SetNameText("");
                            SetMiddleNameText("");
                            SetLastNameText("");
                            SetSecondSurNameText("");
                            SetCelebrateContract("");
                            SetExpirationContract("");
                            SetStatusClientRedText("¿Tienes huella digital registrada ? ???");
                            SetMessageQueueSnackBar("Cliente no tiene huella registrada o no esta registrado.");
                        }
                    }
                    else
                    {
                        SetMessageQueueSnackBar("Cliente no encontrado o no presenta huella registrada.");
                        szMessage.Append("No encontrado");
                    }
                }
                else
                {
                    szMessage.Append("Identificación fallido.");
                    szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult));
                }
                this.SetIdentificationLimit(m_Operation.IdentificationsLeft);
            }
            else
            {
                szMessage.Append("No se puede recuperar la plantilla base.");
                szMessage.Append("Error descripción: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
            }
            this.SetStatusText(szMessage.ToString());

            // start identification process
            ((FutronicIdentification)m_Operation).GetBaseTemplate();
        }


        private void SetIdentificationLimit(int nLimit)
        {
            if (this.IdentificationLimits.Dispatcher.Thread == Thread.CurrentThread)
            {
                if (nLimit == Int32.MaxValue)
                {
                    IdentificationLimits.Content = "Identificacion limit: No limits";
                }
                else
                {
                    IdentificationLimits.Content = String.Format("Identificación limit: {0}", nLimit);
                }
            }
            else
            {
                SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
                Dispatcher.Invoke(d, new object[] { nLimit });
            }
        }

        private void IdentityApp()
        {
            SetStatusText("El programa está cargando la base de datos, espere...");
            List<DbRecord> Users = DbRecord.ReadRecords(m_DatabaseDir);
            SetStatusText(String.Empty);
            if (Users.Count == 0)
            {
                MessageBox.Show("Usuarios no encontrados. Por favor, ejecute el proceso de inscripción primero.", "KallpaBox Security",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            m_OperationObj = Users;
            FutronicSdkBase dummy = new FutronicIdentification();
            if (m_Operation != null)
            {
                //m_Operation.Dispose();
                m_Operation = null;
            }
            m_Operation = dummy;

            // Set control property
            m_Operation.FakeDetection = true;
            m_Operation.FFDControl = true;
            m_Operation.FARN = 116;
            m_Operation.Version = (VersionCompatible)2;
            m_Operation.FastMode = false;

            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete +=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

            // start identification process
            ((FutronicIdentification)m_Operation).GetBaseTemplate();
        }


        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Pon el dedo en el dispositivo, por favor ...");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Quite el dedo del dispositivo, por favor ...");
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
            result = MessageBox.Show("Fuente falsa detectada o el dispositivo esta sucio. ¿Quieres continuar el proceso?",
                                     "KallpaBox Security.",
                                     MessageBoxButton.YesNo, MessageBoxImage.Question);
            return (result == MessageBoxResult.No);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Operation.OnCalcel();
        }

        private void SetIdentificationText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.Identification.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.Identification.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetIdentificationText);
                Identification.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }

        private void SetMessageQueueSnackBar(string message)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (SnackBarMessagesApp.Dispatcher.Thread == Thread.CurrentThread)
            {
                var messageQueue = SnackBarMessagesApp.MessageQueue;
                Task.Factory.StartNew(() => messageQueue.Enqueue(message));
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetMessageQueueSnackBar);
                SnackBarMessagesApp.Dispatcher.BeginInvoke(d, new object[] { message });
            }
        }

        private void SetNameText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.Name.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.Name.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetNameText);
                Name.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }
        private void SetMiddleNameText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.MiddleName.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.MiddleName.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetMiddleNameText);
                MiddleName.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }
        private void SetLastNameText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.LastName.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.LastName.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetLastNameText);
                LastName.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }
        private void SetSecondSurNameText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.SecondSurName.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.SecondSurName.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetSecondSurNameText);
                SecondSurName.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }

        private void SetCelebrateContract(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.DateCelebrate.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.DateCelebrate.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetCelebrateContract);
                DateCelebrate.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }


        private void SetExpirationContract(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.DateExpiration.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.DateExpiration.Content = text;
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetExpirationContract);
                DateExpiration.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }


        private void SetStatusClientRedText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.Status.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.Status.Content = text;
                this.Status.Foreground = new SolidColorBrush(Colors.Red);
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetStatusClientRedText);
                Status.Dispatcher.BeginInvoke(d, new object[] { text });
            }
        }

        private void SetStatusClientGreenText(string text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            if (this.Status.Dispatcher.Thread == Thread.CurrentThread)
            {
                this.Status.Content = text;
                this.Status.Foreground = new SolidColorBrush(Colors.Green);
                this.UpdateLayout();
            }
            else
            {
                SetTextCallback d = new SetTextCallback(this.SetStatusClientRedText);
                Status.Dispatcher.BeginInvoke(d, new object[] { text });
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
                //// unregister events
                m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
                m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
                m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
                m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
                ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
                        new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

                m_OperationObj = null;
                m_Operation.Dispose();
            }
        }


        private void BackToListSessionsGym_Click(object sender, RoutedEventArgs e)
        {
        }

        private void VerifiIdentity_Click(object sender, RoutedEventArgs e) { }


    }
}
