using System;
using MaterialDesignThemes.Wpf;
using Site.Constants;
using Site.Pages.Access;
using Site.Pages.Clients;
using Site.Pages.DashBoard;
using Site.Pages.FingerPrints;
using Site.Pages.ServiceGymContracts;
using Site.Pages.ServiceGyms;
using Site.Pages.ServiceGymTypes;


namespace Site.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(ISnackbarMessageQueue snackbarMessageQueue)
        {
            try
            {
                if (snackbarMessageQueue == null) throw new ArgumentNullException(nameof(snackbarMessageQueue));

                ItemMenus = new[]
                {
                    new MenusItems(ConstantsDashBoard.NameWindowDashBoard,typeof(DashBoard)),
                    new MenusItems(ConstantsClients.NameWindowClientsList, typeof(ClientsList)),
                    new MenusItems(ConstantsAccessPage.NameWindowAccessPage, typeof (AccessPage)),
                    new MenusItems(ConstantsFingerPrints.NameWindowFingerPrints, typeof (FingerPrintsCreate)),
                    new MenusItems(ConstantsServiceGym.NameWindowServiceGymsList, typeof (ServiceGymsList)),
                    new MenusItems(ConstantsServiceGymType.NameWindowServiceGymTypeList, typeof (ServiceGymTypesList)),
                    new MenusItems(ConstantsServiceGymContracts.NameWindowServiceGymContractsList, typeof (ServiceGymContractsList))
                };
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Error {0}", ex.Message), ex);
            }
        }

        public MenusItems[] ItemMenus { get; }
    }
}