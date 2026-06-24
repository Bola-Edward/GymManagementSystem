using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.Data.Contexts;
using GymManagementSystem.DataAccess.Repositories;
using GymManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymManagementSystem.Controllers
{
    public class PlansController : Controller
    {
        private readonly IRepository<Plan> _planRepository;

        public PlansController(IRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index()
        {
            var plans = await _planRepository.GetAllAsync();

            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var plan = await _planRepository.GetByIdAsync(id, cancellationToken);

            if (plan is null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(plan);  // Views/Plans/Details.cshtml
        }
    }
}