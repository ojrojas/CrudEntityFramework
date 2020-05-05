using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;

namespace KallpaBox.Core.Entities
{
    public class FingerPrint : BaseEntity
    {
        public string Finger { get; set; }
        public string identityGuid { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }

    }
}