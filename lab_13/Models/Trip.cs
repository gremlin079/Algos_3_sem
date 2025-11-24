using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TaxiParkAppMobile1.Models;

public class Trip
{
    public int Id { get; set; }

    [Required]
    public string FromAddress { get; set; } = string.Empty;

    [Required]
    public string ToAddress { get; set; } = string.Empty;

    public DateTime TripDate { get; set; } = DateTime.Now;

    [Precision(10, 2)]
    public decimal Price { get; set; }

    public int DriverId { get; set; }

    public int CarId { get; set; }

    public Driver? Driver { get; set; }

    public Car? Car { get; set; }
}

