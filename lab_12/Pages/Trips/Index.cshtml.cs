using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaxiCompanyApp.Data;
using TaxiCompanyApp.Models;

namespace TaxiCompanyApp.Pages.Trips
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public IList<Trip> Trips { get; set; } = default!;

        public async Task OnGetAsync()
        {
            Trips = await _context.Trips
                .Include(t => t.Driver)
                .Include(t => t.Car)
                .ToListAsync();
        }
    }
}