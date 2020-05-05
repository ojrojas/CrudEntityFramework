using KallpaBox.Core.Entities;
namespace Site.ViewModels
{
    public class PaymentMethodViewModel : BaseEntity
    {
        public string Alias { get; set; }
        public string CardId { get; set; } 
        public string Last4 { get; set; }
    }
}