using KallpaBox.Core.Entities;

namespace Site.ViewModels
{
    public class PhotoViewModel : BaseEntity
    {
        public string PhotoClient { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
    }
}
