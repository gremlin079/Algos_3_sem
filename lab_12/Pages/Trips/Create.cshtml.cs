using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TaxiCompanyApp.Data;
using TaxiCompanyApp.Models;

namespace TaxiCompanyApp.Pages.Trips
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Trip Trip { get; set; } = new Trip();

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FirstName");
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "LicensePlate");
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                _context.Trips.Add(Trip);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ViewData["DriverId"] = new SelectList(_context.Drivers, "Id", "FirstName");
                ViewData["CarId"] = new SelectList(_context.Cars, "Id", "LicensePlate");
                ModelState.AddModelError("", $"Ошибка: {ex.Message}");
                return Page();
            }
        }
    }
}