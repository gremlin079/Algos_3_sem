using System.ComponentModel.DataAnnotations;

namespace TaxiCompanyApp.Models
{
    public class Driver
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [Display(Name = "Телефон")]
        public string Phone { get; set; }

        [Display(Name = "Лицензия")]
        public string LicenseNumber { get; set; }

        [Display(Name = "Стаж")]
        public int Experience { get; set; }

        // Навигационное свойство
        public ICollection<Trip> Trips { get; set; }
    }
}