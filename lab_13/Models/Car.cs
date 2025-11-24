using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiParkAppMobile1.Models;

public class Car
{
    public int Id { get; set; }

    [Required]
    public string Brand { get; set; } = string.Empty;

    [Required]
    public string Model { get; set; } = string.Empty;

    public string LicensePlate { get; set; } = string.Empty;

    public int Year { get; set; }

    public string Color { get; set; } = string.Empty;

    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    public override string ToString() => $"{Brand} {Model} ({LicensePlate})";
}

