using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.BusinessLogic.ViewModels.TrainerViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.Presentation.Controllers
{
    public class TrainersController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainersController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
            => View(await _trainerService.GetAllTrainersAsync(cancellationToken));

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(CreateTrainerViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.CreateTrainerAsync(model, cancellationToken);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Trainer Failed create";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
        {
            var trainer = await _trainerService.GetTrainerToUpdateAsync(id, cancellationToken);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditTrainerViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _trainerService.UpdateTrainerDetailsAsync(id, model, cancellationToken);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = "Trainer Failed To update";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var trainer = await _trainerService.GetTrainerDetailsAsync(id, cancellationToken);
            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken cancellationToken)
        {
            var result = await _trainerService.RemoveTrainerAsync(id, cancellationToken);
            if (result)
            {
                TempData["SuccessMessage"] = "Trainer deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To delete Trainer. Ensure they have no future sessions.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
