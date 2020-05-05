using System.Collections.Generic;
using KallpaBox.Core.Entities;
using Site.ViewModels;

namespace Site.ViewModels
{
    public class ServiceGymViewModel :BaseEntity
    {
        public string Name { get; set; }
        public int ServiceGymTypeId { get; set; }
        public ServiceGymType ServiceGymType { get; set; }
        public IEnumerable<ServiceGymTypeViewModel> ServiceGymTypes { get; set; }
    }
}