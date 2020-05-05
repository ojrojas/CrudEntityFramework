using System.Collections.Generic;
using KallpaBox.Core.Interfaces;
using Ardalis.GuardClauses;

namespace KallpaBox.Core.Entities
{
    public class Client :BaseEntity, IAggregateRoot
    {
        public string IdentityGuid { get; set; }
        public string Identification { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string SecondSurName { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        private List<PaymentMethod> _paymentMethods = new List<PaymentMethod>(); 
        public IEnumerable<PaymentMethod> PaymentMethods  => _paymentMethods.AsReadOnly();
    }
}