using System;

namespace KallpaBox.Core.Entities
{
    public class SessionGym : BaseEntity
    {
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public DateTime StartSession { get; set; } = DateTime.Now;
        public DateTime EndSession { get; set; }
    }
}