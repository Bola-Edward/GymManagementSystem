using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var result = await _sessionService.GetAllSessionsAsync(cancellationToken);

            if (result.Success)
            {
                return View(result.Value);
            }


            return View(new List<SessionViewModel>());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {

            var result = await _sessionService.GetSessionByIdAsync(id, cancellationToken);


            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }


            return View(result.Value);
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await PopulateDropdownsAsync(cancellationToken);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSessionViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(cancellationToken);
                return View(model);
            }

            var result = await _sessionService.CreateSessionAsync(model, cancellationToken);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.Error;

            await PopulateDropdownsAsync(cancellationToken);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var session = await _sessionService.GetSessionToUpdateAsync(id, cancellationToken);
            if (session is null)
            {
                TempData["ErrorMessage"] = "Session cannot be edited (not found, already started, or has bookings).";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdownsAsync(cancellationToken);
            return View(session);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditSessionViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(cancellationToken);
                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, cancellationToken);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownsAsync(cancellationToken);
            return View(model);
        }

        private async Task PopulateDropdownsAsync(CancellationToken cancellationToken)
        {
            ViewBag.Trainers = new SelectList(await _sessionService.GetTrainersForDropDownAsync(cancellationToken), "Id", "Name");
            ViewBag.Categories = new SelectList(await _sessionService.GetCategoriesForDropDownAsync(cancellationToken), "Id", "CategoryName");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var result = await _sessionService.GetSessionToDeleteAsync(id, cancellationToken);
            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }
            return View(result.Value);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _sessionService.DeleteAsync(id, cancellationToken);


            if (!result.Success)
            {
                TempData["ErrorMessage"] = result.Error;
                return RedirectToAction(nameof(Index));
            }


            TempData["SuccessMessage"] = "Session deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
