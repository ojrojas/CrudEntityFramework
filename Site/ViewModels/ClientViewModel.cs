using KallpaBox.Core.Entities;

namespace Site.ViewModels
{
    public class ClientViewModel : BaseEntity
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
    }
}