using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaxiParkAppMobile1.Models;

public class Driver
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public string LicenseNumber { get; set; } = string.Empty;

    public int Experience { get; set; }

    public ICollection<Trip> Trips { get; set; } = new List<Trip>();

    public override string ToString() => $"{FirstName} {LastName}";
}

