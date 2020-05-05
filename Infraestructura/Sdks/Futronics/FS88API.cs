using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Futronic.SDKHelper;
using KallpaBox.Core.Interfaces;
using KallpaBox.Infraestructura.Sdks.Futronics;

namespace Futronic.Devices.FS88
{
    public class FutronicApi 
    {
        //private readonly IAppLogger<FutronicApi> _logger;
        const string kCompanyName = Constants.kCompanyName;
        const string kProductName = Constants.kProductName;
        const string kDbName = Constants.kDbName;

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a status control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetTextCallback(string text);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the text property on a identification limit control.
        /// </summary>
        /// <param name="text"></param>
        delegate void SetIdentificationLimitCallback(int limit);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Image property on a PictureBox control.
        /// </summary>
        /// <param name="hBitmap">the instance of Bitmap class</param>
        delegate void SetImageCallback(Bitmap hBitmap);

        /// <summary>
        /// This delegate enables asynchronous calls for setting
        /// the Enable property on a buttons.
        /// </summary>
        /// <param name="bEnable">true to enable buttons, otherwise to disable</param>
        delegate void EnableControlsCallback(bool bEnable);

        public FutronicApi()
        {
            m_bInitializationSuccess = false;
            m_bExit = false;
            //_logger = logger;
            try
            {
                m_DatabaseDir = GetDatabaseDir();
                //_logger.LogInformation("Se crear el directorio de base de datos.");
            }
            catch (UnauthorizedAccessException uaException)
            {
                throw new Exception("Error al dar autorizacion al dispositivo, message: " + uaException.Message);
            }
            catch (IOException ioException)
            {
                throw new Exception("Error de entrada y salida de datos: " + ioException.Message);
            }

              m_bInitializationSuccess = true;
        }

        /// <summary>
        /// Contain reference for current operation object
        /// </summary>
        private FutronicSdkBase m_Operation;

        private bool m_bExit;

        /// <summary>
        /// The type of this parameter is depending from current operation. For
        /// enrollment operation this is DbRecord.
        /// </summary>
        private object m_OperationObj;

        /// <summary>
        /// A directory name to write user's information.
        /// </summary>
        private string m_DatabaseDir;

        private bool m_bInitializationSuccess;

        private void SetIdentificationLimit(int nLimit)
        {
                SetIdentificationLimitCallback d = new SetIdentificationLimitCallback(this.SetIdentificationLimit);
                // this.Invoke(d, new object[] { nLimit });
        }


        public void Enroll(string name, int max_models = 2)
        {
            DbRecord _user = new DbRecord();
            if (name == null)
            {
                //_logger.LogWarning("Error se esperaba el paramentro nombre");
                return;
            }

            if(name.Length == 0)
            {
                //_logger.LogWarning("Error el paranombre deberia tener caracteres");
                return;
            }

            if(IsUserExists(name))
            {
                //_logger.LogInformation("El usuario existe en la base de datos");
            }
            else
            {
                try
                {
                    CreateFile(name);
                }
                catch (DirectoryNotFoundException dnfException)
                {
                //_logger.LogWarning(string.Format("Error el directorio no existe, {0}", dnfException));

                    return;
                }
                catch (IOException ioException)
                {
                //_logger.LogWarning(string.Format("Error de lectura y escritura, {0}",ioException));
                    return;
                }
            }

            _user.UserName = name;
            m_OperationObj = _user;
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
            m_Operation.FARN = 2;
            m_Operation.Version = VersionCompatible.ftr_version_current;
            m_Operation.FastMode = true;
            ((FutronicEnrollment)m_Operation).MIOTControlOff = false;
            ((FutronicEnrollment)m_Operation).MaxModels = max_models;


            // register events
            m_Operation.OnPutOn += new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff += new OnTakeOffHandler(this.OnTakeOff);
            //m_Operation.UpdateScreenImage += new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource += new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete += new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            // start enrollment process
            ((FutronicEnrollment)m_Operation).Enrollment();

        }


        private void OnPutOn(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Put finger into device, please ...");
        }

        private void OnTakeOff(FTR_PROGRESS Progress)
        {
            this.SetStatusText("Take off finger from device, please ...");
        }

        private void UpdateScreenImage(Bitmap hBitmap)
        {
            if (hBitmap is null)
            {
                throw new ArgumentNullException(nameof(hBitmap));
            }
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            //if (PictureFingerPrint.InvokeRequired)
            //{
            SetImageCallback d = new SetImageCallback(this.UpdateScreenImage);

            d.Invoke(hBitmap);
          //  this.Invoke(d, new object[] { hBitmap });
            //}
            //else
            //{
            //    PictureFingerPrint.Image = hBitmap;
            //}
        }

        private bool OnFakeSource(FTR_PROGRESS Progress)
        {
            if (m_bExit)
                return true;
            else return false;

            //DialogResult result;
            //result = MessageBox.Show("Fake source detected. Do you want continue process?",
            //                         "C# example for Futronic SDK",
            //                         MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //return (result == DialogResult.No);
        }

        private void OnEnrollmentComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                // set status string
                szMessage.Append("Enrollment process finished successfully.");
                szMessage.Append("Quality: ");
                szMessage.Append(((FutronicEnrollment)m_Operation).Quality.ToString());
                this.SetStatusText(szMessage.ToString());

                // Set template into user's information and save it
                DbRecord User = (DbRecord)m_OperationObj;
                User.Template = ((FutronicEnrollment)m_Operation).Template;

                String szFileName = Path.Combine(m_DatabaseDir, User.UserName);
                if (!User.Save(szFileName))
                {
                    //MessageBox.Show("Can not save users's information to file " + szFileName,
                    //                 "C# example for Futronic SDK",
                    //                 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                szMessage.Append("Enrollment process failed.");
                szMessage.Append("Error description: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
                this.SetStatusText(szMessage.ToString());
            }

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            //m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicEnrollment)m_Operation).OnEnrollmentComplete -= new OnEnrollmentCompleteHandler(this.OnEnrollmentComplete);

            m_OperationObj = null;
            EnableControls(true);
        }

        private void OnVerificationComplete(bool bSuccess,
                                            int nRetCode,
                                            bool bVerificationSuccess)
        {
            StringBuilder szResult = new StringBuilder();
            if (bSuccess)
            {
                if (bVerificationSuccess)
                {
                    szResult.Append("Verification is successful.");
                    szResult.Append("User Name: ");
                    szResult.Append(((DbRecord)m_OperationObj).UserName);
                }
                else
                    szResult.Append("Verification failed.");
            }
            else
            {
                szResult.Append("Verification process failed.");
                szResult.Append("Error description: ");
                szResult.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
            }

            this.SetStatusText(szResult.ToString());
            this.SetIdentificationLimit(m_Operation.IdentificationsLeft);

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            //m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicVerification)m_Operation).OnVerificationComplete -= new OnVerificationCompleteHandler(this.OnVerificationComplete);

            m_OperationObj = null;
            EnableControls(true);
        }

        private void OnGetBaseTemplateComplete(bool bSuccess, int nRetCode)
        {
            StringBuilder szMessage = new StringBuilder();
            if (bSuccess)
            {
                this.SetStatusText("Starting identification...");
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
                    szMessage.Append("Identification process complete. User: ");
                    if (iRecords != -1)
                        szMessage.Append(Users[iRecords].UserName);
                    else
                        szMessage.Append("not found");
                }
                else
                {
                    szMessage.Append("Identification failed.");
                    szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nResult));
                }
                this.SetIdentificationLimit(m_Operation.IdentificationsLeft);
            }
            else
            {
                szMessage.Append("Can not retrieve base template.");
                szMessage.Append("Error description: ");
                szMessage.Append(FutronicSdkBase.SdkRetCode2Message(nRetCode));
            }
            this.SetStatusText(szMessage.ToString());

            // unregister events
            m_Operation.OnPutOn -= new OnPutOnHandler(this.OnPutOn);
            m_Operation.OnTakeOff -= new OnTakeOffHandler(this.OnTakeOff);
            //m_Operation.UpdateScreenImage -= new UpdateScreenImageHandler(this.UpdateScreenImage);
            m_Operation.OnFakeSource -= new OnFakeSourceHandler(this.OnFakeSource);
            ((FutronicIdentification)m_Operation).OnGetBaseTemplateComplete -=
                    new OnGetBaseTemplateCompleteHandler(this.OnGetBaseTemplateComplete);

            m_OperationObj = null;
            EnableControls(true);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            m_Operation.OnCalcel();
        }

        private void EnableControls(bool bEnable)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;
            //if (this.InvokeRequired)
            //{
            //    EnableControlsCallback d = new EnableControlsCallback(this.EnableControls);
            //    this.Invoke(d, new object[] { bEnable });
            //}
            //else
            //{
            //    btnEnroll.Enabled = bEnable;
            //    btnIdentify.Enabled = bEnable;
            //    btnVerify.Enabled = bEnable;
            //    btnStop.Enabled = !bEnable;
            //}
        }

        private void SetStatusText(String text)
        {
            // Do not change the state control during application closing.
            if (m_bExit)
                return;

            //if (this.txtMessage.InvokeRequired)
            //{
            //    SetTextCallback d = new SetTextCallback(this.SetStatusText);
            //    this.Invoke(d, new object[] { text });
            //}
            //else
            //{
            //    this.txtMessage.Text = text;
            //    this.Update();
            //}
        }

        /// <summary>
        /// Get the database directory.
        /// </summary>
        /// <returns>returns the database directory.</returns>
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


        protected bool IsUserExists(string UserName)
        {
            string szFileName;
            szFileName = Path.Combine(this.m_DatabaseDir, UserName);
            return File.Exists(szFileName);
        }

        protected void CreateFile(string UserName)
        {
            string szFileName;
            szFileName = Path.Combine(m_DatabaseDir, UserName);
            File.Create(szFileName).Close();
            File.Delete(szFileName);
        }

        /// <summary>
        /// Operacion al cerrar la vista razor lectora de la Huella
        /// </summary>
        public void ToClosed()
        {
            m_bExit = true;
            if (m_Operation != null)
            {
                m_Operation.Dispose();
            }
        }
    }
}