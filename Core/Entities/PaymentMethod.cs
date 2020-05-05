using KallpaBox.Core.Entities;
using KallpaBox.Core.Interfaces;

namespace KallpaBox.Core.Entities
{
    public class PaymentMethod : BaseEntity
    {
        public string Alias { get; set; }
        public string CardId { get; set; } 
        public string Last4 { get; set; }
    }
}