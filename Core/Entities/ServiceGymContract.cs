using KallpaBox.Core.Entities;
using System;

namespace KallpaBox.Core.Entities
{
    public class ServiceGymContract : BaseEntity
    {
        public ServiceGym ServiceGym { get; set; }
        public int ServiceGymId { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public Decimal Price { get; set; }
        public int TypeQuantity { get; set; }
        public int Quantity { get; set; }
        public DateTime DateCelebrate { get; set; } = DateTime.Now;
        public DateTime DateExpiration { get; set; }
        public bool State { get; set; } 
    }
}