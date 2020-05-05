namespace KallpaBox.Core.Entities
{
    public class Photo : BaseEntity
    {
        public string PhotoClient { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public long LengthPhoto { get; set; }
    }
}