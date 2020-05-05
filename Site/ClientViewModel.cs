using System.ComponentModel.DataAnnotations;
using KallpaBox.Core.Entities;

namespace KallpaBox.Site.ViewModels
{
    public class ClientViewModel : BaseEntity
    {

        public string IdentityGuid { get; private set; }
        [Required]
        [MaxLength(length: 12, ErrorMessage = "El campo identificacion no debe ser mayor a 12 caracteres")]
        public string Identification { get; set; }
        [Required]
        [MaxLength(length: 50, ErrorMessage = "El campo nombre no debe ser mayor a 50 caracteres")]
        public string Name { get; set; }
        [MaxLength(length: 50, ErrorMessage = "El campo segundo nombre no debe ser mayor a 50 caracteres")]
        [Display(Name="Middle Name")]
        public string MiddleName { get; set; }
        [Required]
        [MaxLength(length: 50, ErrorMessage = "El campo apellido no debe ser mayor a 50 caracteres")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [MaxLength(length: 50, ErrorMessage = "El campo segundo apellido no debe ser mayor a 50 caracteres")]
        [Display(Name="Second SurName")]
        public string SecondSurName { get; set; }
        [Required]
        [MaxLength(length: 14, ErrorMessage = "El campo teléfono no debe ser mayor a 50 caracteres")]
        public string Phone { get; set; }
        [Required]
        [Range(minimum: 0, maximum: 150, ErrorMessage = "El campo edad debe ser menor a 0 y mayor 150")]
        public int Age { get; set; }
        [Required]
        [MaxLength(length: 400, ErrorMessage = "El campo nombre no debe ser mayor a 50 caracteres")]
        public string Address { get; set; }
    }
}