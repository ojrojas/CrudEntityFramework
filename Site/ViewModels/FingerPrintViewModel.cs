using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace Site.ViewModels
{
    public class FingerPrintViewModel : BaseEntity
    {
        public string Finger { get; set; }
        public string identityGuid { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
    }
}