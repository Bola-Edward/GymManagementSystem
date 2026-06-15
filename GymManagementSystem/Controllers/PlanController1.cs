using GymManagementSystem.Data.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        public GymDbContext dbContext = new GymDbContext();
        public async Task<IActionResult> Index()
        {
            var plans = await dbContext.Plans.ToListAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var plan = await dbContext.Plans.FirstOrDefaultAsync(p => p.Id == id);

            if (plan is null)
            {
                return RedirectToAction(nameof(Index)); // return 302 location
            }

            return View(plan);  // Views/Plans/Details.cshtml
        }
    }
}
