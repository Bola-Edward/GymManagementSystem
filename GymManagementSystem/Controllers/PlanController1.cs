using GymManagementSystem.Data.Contexts;
using GymManagementSystem.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IPlanRepository _repo;

        public PlansController(IPlanRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _repo.GetAllAsync();
            return View(plans);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var plan = await _repo.GetByIdAsync(id);

            if (plan is null)
            {
                return RedirectToAction(nameof(Index)); // return 302 location
            }

            return View(plan);  // Views/Plans/Details.cshtml
        }
    }
}