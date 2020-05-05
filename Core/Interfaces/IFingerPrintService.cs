using KallpaBox.Core.Entities;
using System.Threading.Tasks;

namespace KallpaBox.Core.Interfaces
{
    public interface IFingerPrintService
    {
        void CreateFingerPrint(FingerPrint fingerPrint);
        FingerPrint GetFingerPrintById(int fingerPrintId);
    }
}