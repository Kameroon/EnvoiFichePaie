using MMA.Prism.Infrastructure.Communs;
using MMA.Prism.Infrastructure.Services;
using MMA.Prism.ModuleEnvoiFichePaie.Helpers;
using MMA.Prism.ModuleEnvoiFichePaie.MVVM.Interfaces;
using MMA.Prism.ModuleEnvoiFichePaie.Resources;
using MMA.Prism.ModuleEnvoiFichePaie.Services;
using NLog;
using Prism.Commands;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using MMA.Prism.Infrastructure.Communs.Helpers;

//https://stackoverflow.com/questions/1750934/best-logging-approach-for-composite-app
// https://github.com/NLog/NLog/wiki/Tutorial
// https://stackify.com/nlog-guide-dotnet-logging/

namespace MMA.Prism.ModuleEnvoiFichePaie.MVVM.ViewModels
{
    public class EnvoiFichePaieViewModel : ViewModelBase, IDisposable
    {
        private string ErrorMessage { get; set; }
        private string InfoMessage { get; set; }

        private DataTable MYDataTable = new DataTable();
        private Dictionary<string, string> MyDictionnary = new Dictionary<string, string>();
        public readonly IRegionManager _regionManager;
        private BackgroundWorker bgWorkerExport;

        #region -- Privates -- 
        private string _bccMail = "citoyenlamda@gmail.com";
        private string _ccMail; 
        private string _adminEmail;
        private string _mailTemplate;
        private bool _isBccOrCcEmpty;
        private string _filePath;
        private bool _isTestMail;
        private bool _isFilePathVisible;
        private bool _isFilePathCorrect;
        private bool _isProgressVisibility;
        private double _currentProgress;
        private bool _canContinuousSendMail;
        private bool _isMainGridEnable;
        private bool _isAdminEmailValidVar;
        private bool _checkIfCanSendMail;
        #endregion
       
        private readonly string corpsDuMail = @"C:\Users\Sweet Family\Desktop\Mail Afersys\Mail Templates\Template.htm";
               
        //public DelegateCommand<string> NavigateCommand { get; set; }
        //public DelegateCommand<object> CloseTabCommand { get; }               

        #region -- Labels Initailization --
        private string _applicationLoadingLabel = ModuleEnvoiFichePaieLabels.ModuleLoadingLabel;
        public string ApplicationLoadingLabel
        {
            get { return _applicationLoadingLabel; }
            set { _applicationLoadingLabel = value; }
        }

        public string BrowseLabelBtn { get; private set; } = ModuleEnvoiFichePaieLabels.BrowseLabelBtn;
        public string SendPreviewLabelBtn { get; private set; } = ModuleEnvoiFichePaieLabels.SendPreviewLabelBtn;
        public string SendByMailLabelBtn { get; private set; } = ModuleEnvoiFichePaieLabels.SendByMailLabelBtn;
        public string ClearBccCcBoxLabelBtn { get; private set; } = ModuleEnvoiFichePaieLabels.ClearBccCcBoxLabelBtn;

        #endregion

        public ICommand CleerBccOrCcMailCommand { get; private set; }
        public ICommand SendPreviewCommand { get; private set; }
        public ICommand SendMailCommand { get; private set; }
        public ICommand BrowseCommand { get; private set; }

        #region -- ProgressBar properties --
        private string _progressValue;
        public string ProgressValue
        {
            get { return _progressValue; }
            set { SetProperty(ref _progressValue, value); }
        }
        #endregion

        private DateTime CurrentDateTime = DateTime.Now;
        private readonly IDialogService _dialogService;
        //private readonly IMessageBoxConsolidateHelper _messageBoxConsolidateHelper;
        private IEmailMessage _emailMessage = null;

        #region -- Properties -- 
        public bool CheckIfCanSendMail
        {
            get { return _checkIfCanSendMail; }
            set { SetProperty(ref _checkIfCanSendMail, value); }
        }

        public bool IsAdminEmailValid
        {
            get { return _isAdminEmailValidVar; }
            set { SetProperty(ref _isAdminEmailValidVar, value); }
        }

        public string MailTemplate
        {
            get { return _mailTemplate; }
            set { _mailTemplate = value; }
        }

        public string BccMail
        {
            get { return _bccMail; }
            set
            {
                SetProperty(ref _bccMail, value);
                IsBccOrCcEmpty = CheckEmailContent(BccMail);
            }
        }

        public string CcMail
        {
            get { return _ccMail; }
            set
            {
                SetProperty(ref _ccMail, value);
                IsBccOrCcEmpty = CheckEmailContent(CcMail);
            }
        }

        public string AdminEmail
        {
            get { return _adminEmail; }
            set
            {
                SetProperty(ref _adminEmail, value);
            }
        }

        public bool IsBccOrCcEmpty
        {
            get { return _isBccOrCcEmpty; }
            set { SetProperty(ref _isBccOrCcEmpty, value); }
        }

        public string FilePath
        {
            get { return _filePath; }
            set { SetProperty(ref _filePath, value); }
        }

        public bool IsPreviewEmail
        {
            get { return _isTestMail; }
            set { SetProperty(ref _isTestMail, value); }
        }

        public bool IsFilePathVisible
        {
            get { return _isFilePathVisible; }
            set { SetProperty(ref _isFilePathVisible, value); }
        }

        public bool IsFilePathCorrect
        {
            get { return _isFilePathCorrect; }
            set { SetProperty(ref _isFilePathCorrect, value); }
        }

        public bool IsProgressBarVisible
        {
            get { return _isProgressVisibility; }
            set { SetProperty(ref _isProgressVisibility, value); }
        }

        public double CurrentProgress
        {
            get { return _currentProgress; }
            private set
            {
                if (_currentProgress != value)
                {
                    SetProperty(ref _currentProgress, value);
                }
            }
        }

        private int CountData { get; set; }

        public bool CanContinuousSendMail
        {
            get { return _canContinuousSendMail; }
            set { SetProperty(ref _canContinuousSendMail, value); }
        }

        public bool IsMainGridEnable
        {
            get { return _isMainGridEnable; }
            set { SetProperty(ref _isMainGridEnable, value); }
        }
        #endregion  
       
        #region -- Contructor --
        public EnvoiFichePaieViewModel(IRegionManager regionManager,
            IDialogService dialogService,
            IEmailMessage emailMessage)
        {
            _logger.Debug($"-- ******************** Demarrage du module [ModuleEnvoiFichePaie] ...******************** --");

            CheckIfCanSendMail = false;

            IsPreviewEmail = false;
            IsBccOrCcEmpty = true;
            IsFilePathVisible = false;
            IsMainGridEnable = true;
            IsProgressBarVisible = false;

            _regionManager = regionManager;
            _dialogService = dialogService;
            _emailMessage = emailMessage;

            //NavigateCommand = new DelegateCommand<string>(Navigate);

            MailTemplate = corpsDuMail != null ? File.ReadAllText(corpsDuMail, Encoding.Default) : null;

            _logger.Debug($"==> Debut Initialisation des commandes...");
            BrowseCommand = new DelegateCommand<object>(OnBrowse, CanBrowse);
            SendMailCommand = new DelegateCommand<object>(OnSendMail, CanSendMail);
            SendPreviewCommand = new DelegateCommand<object>(OnSendPreview, CanSendPreview);
            CleerBccOrCcMailCommand = new DelegateCommand<object>(OnClearBccAndCcMail, CanClearBccOrCcMail);
            _logger.Debug($"==> Fin Initialisation des commandes...");

            #region -- thread for cut chart --
            bgWorkerExport = new BackgroundWorker();
            bgWorkerExport.WorkerReportsProgress = true;
            bgWorkerExport.DoWork += Export_DoWork;
            bgWorkerExport.RunWorkerCompleted += Export_RunWorkerCompleted;
            bgWorkerExport.ProgressChanged += new ProgressChangedEventHandler(this.Export_ProgressChanged);
            #endregion
        }
        #endregion

        #region -- Methodes --

        #region -- Background Worker --
        /// <summary>
        /// -- perform all work here --
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_DoWork(object sender, DoWorkEventArgs e)
        {
            IsProgressBarVisible = true;
            IsMainGridEnable = false;
            // --  --
            _logger.Debug($"==> Début appel à la méthode de Vérification de l'email.");
            ValidateEmail(BccMail);
            ValidateEmail(CcMail);
            ValidateEmail(AdminEmail);
            _logger.Debug($"==> Fin appel à la méthode de Vérification de l'email.");

            for (int i = 0; i < CountData; i++)
            {
                if (bgWorkerExport.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // -- Perform a time consuming operation and report progress. --
                    System.Threading.Thread.Sleep(CountData);

                    #region -- Build any email content --
                    var item = MyDictionnary.ElementAt(i);
                    var itemKey = item.Key;
                    var itemValue = item.Value;

                    // -- Send mail --
                    if (BuildMailBody(itemKey, itemValue, BccMail, CcMail) == false)
                    {
                        CanContinuousSendMail = false;
                        break;
                    }
                    #endregion

                    bgWorkerExport.ReportProgress((i + 1) * (100 / CountData));
                }
            }
        }

        /// <summary>
        /// track progress - this is where you will Update the ProgressBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // -- Update progressbar --
            CurrentProgress = e.ProgressPercentage;
            ProgressValue = $"{e.ProgressPercentage.ToString()} %";
        }

        /// <summary>
        ///  -- When complete --
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Export_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CanContinuousSendMail)
            {
                InfoMessage = "Fin de l'envoie des emails.";
                _dialogService.ShowMessage(InfoMessage, "Information",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Information,
                                           MessageBoxResult.Yes);

                _logger.Debug($"==> {InfoMessage}.");
            }

            IsProgressBarVisible = false;
            IsMainGridEnable = true;
        }
        #endregion

        /// <summary>
        /// -- Check de la ssisie user de Bccmail et Ccmail --
        /// </summary>
        /// <param name="email"></param>
        private void ValidateEmail(string email)
        {
            // -- $"==> Vérification de email saisie." --
            if ((!string.IsNullOrEmpty(email) && !RegexMailUtilities.IsValidEmail(email)) &&
                !RegexMailUtilities.IsValidEmail(email))
            {
                CanContinuousSendMail = false;
                ErrorMessage = string.Format(ErrorMessageLabels.ImpossibleToSendMailMsg, email);
                //ErrorMessage = $"Impossoble d'envoyer des mails.\n Assurez-vous que l'adresse : [{email}] saisie soit valide.";
                _logger.Error(ErrorMessage);

                _dialogService.ShowMessage(ErrorMessage, "Error",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error,
                                           MessageBoxResult.Yes);
            }
            else
            {
                // -- $"==> Poursuite de l'envoie de mail après vérification de l'email." --
                CanContinuousSendMail = true;
            }
        }

        private bool CheckEmailContent(string email)
        {
            return !string.IsNullOrWhiteSpace(email) || !string.IsNullOrEmpty(email);
        }

        private bool CanClearBccOrCcMail(object arg)
        {
            return CheckEmailContent(BccMail) || CheckEmailContent(CcMail);
        }

        private void OnClearBccAndCcMail(object obj)
        {
            _logger.Debug($"==> Effacement de l'un de l'autre des Bcc ou Cc ou des deux.");
            if (CheckEmailContent(BccMail) || CheckEmailContent(CcMail))
            {
                BccMail = string.Empty;
                CcMail = string.Empty;
            }
        }

        private void OnSendMail(object obj)
        {
            if (CheckIfCanSendMail)
            {
                _logger.Debug($"==> Debut envoie d'email.");
                Console.WriteLine("Send mail.");
                IsPreviewEmail = false;
                bgWorkerExport.RunWorkerAsync();
            }           
        }

        private void OnSendPreview(object obj)
        {            
            _logger.Debug($"==> Debut envoie d'email de test.");
            Console.WriteLine("Send mail to me.");
            IsPreviewEmail = true;
            bgWorkerExport.RunWorkerAsync();
        }

        /// <summary>
        /// -- Choose Excel file, get Datatable and feel dictionarry --
        /// </summary>
        /// <param name="arg"></param>
        private void OnBrowse(object arg)
        {
            string selectedFile = string.Empty;
            OpenFileDialog oDlg = new OpenFileDialog();

            if (DialogResult.OK == oDlg.ShowDialog())
            {
                selectedFile = oDlg.FileName;
                FilePath = selectedFile;
                _logger.Debug($"==> Vérification du chemin d'accès du fichier.");
                if (!string.IsNullOrWhiteSpace(FilePath))
                {
                    IsFilePathVisible = true;
                    IsFilePathCorrect = true;

                    // -- Get datatable --
                    _logger.Debug($"==> Récupération de la datatable.");
                    MYDataTable = DataService.GetDataTableFromExcelFile(selectedFile);

                    // -- Feel Dictionary --         
                    _logger.Debug($"==> Récupération du dictionare consolidé.");
                    MyDictionnary = ConsolidateDataHelper();
                    if (MyDictionnary.Count > 0)
                    {
                        CountData = MyDictionnary.Count;
                        _logger.Debug($"==> Fin de la récupération du dictionare consolidé.");

                        CheckIfCanSendMail = true;
                    }
                    else
                    {
                        ErrorMessage = ErrorMessageLabels.CantConsolidateDictionnaryMsg;
                        _logger.Error($"==> {ErrorMessage}");

                        _dialogService.ShowMessage(ErrorMessage, "Error",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error,
                                              MessageBoxResult.Yes);

                        IsFilePathCorrect = false;
                        IsFilePathVisible = false;
                    }
                }
                else
                {
                    ErrorMessage = ErrorMessageLabels.WrongDirectoryMsg;
                    _logger.Error($"==> {ErrorMessage}");

                    _dialogService.ShowMessage(ErrorMessage, "Error",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error,
                                              MessageBoxResult.Yes);

                    IsFilePathCorrect = false;
                }
            }
        }                

        private bool CanBrowse(object arg)
        {
            return true;
        }

        private bool CanSendPreview(object arg)
        {
            return true;
        }

        private bool CanSendMail(object arg)
        {
            var res = CheckIfCanSendMail;
            return true;
        }

        /// <summary> 
        /// -- Build mail body --
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="itemValue"></param>
        /// <param name="bcc"></param>
        /// <param name="cc"></param>
        private bool BuildMailBody(string itemKey, string itemValue, string bcc = null, string cc = null)
        {
            string currentMonth = DateTime.Now.ToString("MMMM").ToUpper();
            string sujet = $"Fihe de paie de {currentMonth}.";

            //string fullName = itemKey.ToString().Split('@')[0].ToString();

            //string corpsDuMail = $"Bonjour {fullName.ToUpper()},\n\nTu trouvera ci-joint ta fiche de paie de du mois de " +
            //    $"{currentMonth}. \n\nEn te souhaitant bonne réception";                     

            if (IsPreviewEmail && string.IsNullOrEmpty(AdminEmail))
            {
                ErrorMessage = ErrorMessageLabels.CheckAdminEmailMsg;
                _logger.Debug($"==> {ErrorMessage}.");

                _dialogService.ShowMessage(ErrorMessage, "Error",
                                           MessageBoxButton.OK,
                                           MessageBoxImage.Error,
                                           MessageBoxResult.Yes);

                return false;
            }
            else
            {
                // -- Création d'objet mailMessage --
                _emailMessage.MailBody = corpsDuMail;
                _emailMessage.Suject = sujet;
                _emailMessage.FilePath = itemValue;
                _emailMessage.ToEmail = itemKey;
                _emailMessage.Bcc = bcc;
                _emailMessage.Cc = cc;
                _emailMessage.AdminEmail = AdminEmail;
                _emailMessage.IsPreviewMail = IsPreviewEmail;

                var sendMailResponse = SendEmailHelper.SendEmail(_emailMessage);

                if (sendMailResponse == false)
                {
                    ErrorMessage = "Erreur durant l'envoie de l'email. \n Vous devez saisir l'adresse mail administrateur!";
                    _logger.Debug($"==> {ErrorMessage}.");

                    _dialogService.ShowMessage(ErrorMessage, "Error",
                                               MessageBoxButton.OK,
                                               MessageBoxImage.Error,
                                               MessageBoxResult.Yes);
                }
                return sendMailResponse;
            }
        }

        /// <summary>
        /// -- Feel dictionary --
        /// </summary>
        /// <returns>Dictionary<string, string></returns>
        private Dictionary<string, string> ConsolidateDataHelper()
        {
            _logger.Debug($"==> Début consolidation des données dans le dictionaire.");
            var myDictionnary = new Dictionary<string, string>();
            myDictionnary.Clear();

            if (MYDataTable.Rows.Count < 1)
            {
                ErrorMessage = ErrorMessageLabels.NoDataInDataTableMsg;
                _logger.Error($"==> {ErrorMessage}.");
                return null;
            }

            try
            {
                _logger.Debug($"==> Début parcours de dataTable. Vérifiaction de la validité de l'email et autre");
                for (int i = 0; i < MYDataTable.Rows.Count - 1; i++)
                {
                    // -- $"==> Récupération de l'email." --
                    var mail = MYDataTable.Rows[i][0].ToString();

                    // -- $"==> Récupération du chemin de la fiche de paie." --
                    var filePath = MYDataTable.Rows[i][1].ToString();

                    if (!string.IsNullOrWhiteSpace(mail) && !myDictionnary.ContainsKey(mail))
                    {
                        // -- Check if email is valid --
                        if (RegexMailUtilities.IsValidEmail(mail))
                        {
                            // -- Check filePath --
                            if (!string.IsNullOrWhiteSpace(filePath))
                            {
                                if (File.Exists(filePath))
                                {
                                    // -- Get file name --
                                    var fileName = Path.GetFileName(filePath);

                                    // -- Get userName in email --
                                    string fullName = mail.Split('@')[0];

                                    // -- Get userName in file --
                                    var file = fileName.Split('_')[0];

                                    // -- Check correspondance --
                                    // -- $"==> Rafraichissement du dictionaire." --
                                    if (fullName == file)
                                        myDictionnary.Add(mail, filePath);
                                }
                                else
                                {
                                    ErrorMessage = string.Format(ErrorMessageLabels.CheckFilePathMsg, MYDataTable.Rows[i][1].ToString());
                                    _logger.Error($"==> {ErrorMessage}.");
                                }
                            }
                            else
                            {
                                ErrorMessage = string.Format(ErrorMessageLabels.CheckFilePathMsg, MYDataTable.Rows[i][1].ToString());
                                _logger.Error($"==> {ErrorMessage}");
                            }
                        }
                    }
                }
                _logger.Debug($"==> Fin parcours de dataTable. récupération des  données dans le dictionaire.");
            }
            catch (Exception ex)
            {
                ErrorMessage = string.Format(ErrorMessageLabels.AnErreurOccuredMsg, ex.ToString());
                _logger.Error($"==> {ErrorMessage}.");
                throw new Exception(ex.ToString());
            }
            finally
            {
                if (ErrorMessage != null)
                {
                    _dialogService.ShowMessage(ErrorMessage, "Error",
                                              MessageBoxButton.OK,
                                              MessageBoxImage.Error,
                                              MessageBoxResult.Yes);
                }
            }
            _logger.Debug($"==> Fin consolidation des données dans le dictionaire.");
            return myDictionnary;
        }

        /// <summary>
        /// -- !!!!!!!!!!!!!!!!!!!!!!!!!! --
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region -- Dispose methode --
        bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    //dispose managed resources
                }
            }
            //dispose unmanaged resources
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            bgWorkerExport.Dispose();
            NLog.LogManager.Shutdown();
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
