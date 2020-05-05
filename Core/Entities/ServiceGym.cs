using KallpaBox.Core.Entities;

namespace KallpaBox.Core.Entities  
{
    public class ServiceGym : BaseEntity
    {
        public string Name { get; set; }
        public int ServiceGymTypeId { get; set; }
        public ServiceGymType ServiceGymType { get; set; }
    }
}