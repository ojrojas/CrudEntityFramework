using System;
using KallpaBox.Core.Entities;

namespace Site.ViewModels
{
    public class SessionGymViewModel : BaseEntity
    {
        
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public DateTime StartSession { get; set; } = DateTime.Now;
        public DateTime EndSession { get; set; }
    }
}