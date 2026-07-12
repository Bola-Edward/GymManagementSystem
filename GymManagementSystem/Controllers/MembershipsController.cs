using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.MembershipViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    public class MembershipsController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipsController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
            => View(await _membershipService.GetAllMembershipsAsync(cancellationToken));

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            await PopulateDropdownsAsync(cancellationToken);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMemberShipViewModel model, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownsAsync(cancellationToken);
                return View(model);
            }

            var result = await _membershipService.CreateMembershipAsync(model, cancellationToken);
            if (result.Success)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction(nameof(Index));
            }
            TempData["ErrorMessage"] = result.Error;
            await PopulateDropdownsAsync(cancellationToken);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int memberId, CancellationToken cancellationToken)
        {
            var result = await _membershipService.DeleteActiveMembershipAsync(memberId, cancellationToken);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Membership cancelled." : result.Error;
            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdownsAsync(CancellationToken cancellationToken)
        {
            ViewBag.Plans = new SelectList(await _membershipService.GetPlansForDropDownAsync(cancellationToken), "Id", "Name");
            ViewBag.Members = new SelectList(await _membershipService.GetMembersForDropDownAsync(cancellationToken), "Id", "Name");
        }
    }
}
