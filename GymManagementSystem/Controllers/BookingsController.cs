using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.BLL.ViewModels.BookingViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementSystem.PL.Controllers
{
    public class BookingsController : Controller
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }


        public async Task<IActionResult> Index(CancellationToken cancellationToken)
      => View(await _bookingService.GetAllSessionsAsync(cancellationToken));

        [HttpGet]
        public async Task<IActionResult> GetMembersForUpcomingSession(int id, CancellationToken cancellationToken)
          => View(await _bookingService.GetMembersForUpcomingBySessionIdAsync(id, cancellationToken));

        [HttpGet]
        public async Task<IActionResult> GetMembersForOngoingSessions(int id, CancellationToken cancellationToken)
            => View(await _bookingService.GetMembersForOngoingBySessionIdAsync(id, cancellationToken));

        [HttpGet]
        public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
        {
            var members = await _bookingService.GetMembersForDropDownAsync(id, cancellationToken);
            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.SessionId = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBookingViewModel model, CancellationToken cancellationToken)
        {

            var result = await _bookingService.CreateNewBookingAsync(model, cancellationToken);


            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Booking created successfully." : result.Error;


            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = model.SessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int memberId, int sessionId, CancellationToken cancellationToken)
        {
            var result = await _bookingService.CancelBookingAsync(memberId, sessionId, cancellationToken);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Booking cancelled successfully." : result.Error;

            return RedirectToAction(nameof(GetMembersForUpcomingSession), new { id = sessionId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attended(int memberId, int sessionId, CancellationToken cancellationToken)
        {
            var result = await _bookingService.MarkAttendedAsync(memberId, sessionId, cancellationToken);
            TempData[result.Success ? "SuccessMessage" : "ErrorMessage"] =
                result.Success ? "Attendance recorded." : result.Error;

            return RedirectToAction(nameof(GetMembersForOngoingSessions), new { id = sessionId });
        }
    }
}
