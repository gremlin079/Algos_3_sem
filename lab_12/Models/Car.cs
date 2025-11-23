using System.ComponentModel.DataAnnotations;

namespace TaxiCompanyApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Марка")]
        public string Brand { get; set; }

        [Required]
        [Display(Name = "Модель")]
        public string Model { get; set; }

        [Display(Name = "Гос. номер")]
        public string LicensePlate { get; set; }

        [Display(Name = "Год выпуска")]
        public int Year { get; set; }

        [Display(Name = "Цвет")]
        public string Color { get; set; }

        // Навигационное свойство
        public ICollection<Trip> Trips { get; set; }
    }
}