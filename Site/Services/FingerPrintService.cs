using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;
using KallpaBox.Site.Data;

namespace Site.Services
{
    public class FingerPrintService : IFingerPrintService
    {
        private readonly IRepository<FingerPrint> _fingerRepository;

        public FingerPrintService() => _fingerRepository = new EfRepository<FingerPrint>(new GymContext());
        
        public void CreateFingerPrint(FingerPrint fingerPrint)
        {
            _fingerRepository.Add(fingerPrint);
        }

        public FingerPrint GetFingerPrintById(int fingerPrintId)
        {
            return _fingerRepository.GetById(fingerPrintId);
        }
    }
}