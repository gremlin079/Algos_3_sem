using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TaxiCompanyApp.Models
{
    public class Trip
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Откуда")]
        public string FromAddress { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Куда")]
        public string ToAddress { get; set; } = string.Empty;

        [Display(Name = "Дата и время")]
        public DateTime TripDate { get; set; }

        [Display(Name = "Стоимость")]
        [Precision(10, 2)]  // Добавьте эту строку
        public decimal Price { get; set; }

        // Внешние ключи
        [Display(Name = "Водитель")]
        public int DriverId { get; set; }

        [Display(Name = "Автомобиль")]
        public int CarId { get; set; }

        // Навигационные свойства
        public Driver Driver { get; set; } = null!;
        public Car Car { get; set; } = null!;
    }
}