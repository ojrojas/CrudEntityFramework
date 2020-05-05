using System.Collections.Generic;
using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using Site.Interfaces;
using Site.ViewModels;



namespace Site.Services
{
    public class FingerPrintServiceViewModel : IFingerPrintServiceViewModel
    {

#pragma warning disable CS0169 // El campo 'FingerPrintServiceViewModel._clientRepository' nunca se usa
        private readonly IClientService _clientRepository;
#pragma warning restore CS0169 // El campo 'FingerPrintServiceViewModel._clientRepository' nunca se usa
#pragma warning disable CS0169 // El campo 'FingerPrintServiceViewModel._logger' nunca se usa
        private readonly IAppLogger<ClientServiceViewModel> _logger;
#pragma warning restore CS0169 // El campo 'FingerPrintServiceViewModel._logger' nunca se usa
        public void CreateFingerPrint(FingerPrint fingerPrintViewModel)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteFingerPrintView(int? id, FingerPrintViewModel fingerPrintViewModel)
        {
            throw new System.NotImplementedException();
        }

        public void EnrollmentName(string name)
        {
            throw new System.NotImplementedException();
        }

        public IReadOnlyList<FingerPrintViewModel> GetAllFingerPrintViewView()
        {
            throw new System.NotImplementedException();
        }

        public FingerPrintViewModel GetFingerPrintViewViewById(int? id)
        {
            throw new System.NotImplementedException();
        }

        public void UpateFingerPrintView(int? id, FingerPrintViewModel fingerPrintViewModel)
        {
            throw new System.NotImplementedException();
        }
    }
}