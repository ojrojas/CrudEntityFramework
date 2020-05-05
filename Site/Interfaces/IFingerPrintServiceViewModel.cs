using System.Collections.Generic;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.Interfaces
{
    public interface IFingerPrintServiceViewModel
    {
       
        void CreateFingerPrint(FingerPrint fingerPrintViewModel);
        void UpateFingerPrintView(int? id, FingerPrintViewModel fingerPrintViewModel);
        void DeleteFingerPrintView(int? id, FingerPrintViewModel fingerPrintViewModel);
        FingerPrintViewModel GetFingerPrintViewViewById(int? id);
        IReadOnlyList<FingerPrintViewModel> GetAllFingerPrintViewView();
        void EnrollmentName(string name);
    }
}