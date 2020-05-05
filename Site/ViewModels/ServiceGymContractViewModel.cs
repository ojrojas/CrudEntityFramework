using System;
using System.Collections.Generic;
using KallpaBox.Core.Entities;

namespace Site.ViewModels
{
    public class ServiceGymContractViewModel : BaseEntity
    {
        public ServiceGym ServiceGym { get; set; }
        public int ServiceGymId { get; set; }
        public Client Client { get; set; }
        public int ClientId { get; set; }
        public Decimal Price { get; set; }
        public int TypeQuantity { get; set; }
        public int Quantity { get; set; }
        public bool State { get; set; }
        public string TypeQuantityText { get; set; }
        public string QuantityText { get; set; }

        public DateTime DateCelebrate { get; set; } = DateTime.Now;
        public DateTime DateExpiration { get; set; } 
        public IEnumerable<ServiceGymViewModel> ServiceGyms { get; set; }
        public IEnumerable<ClientViewModel> Clients { get; set; }

    }
}