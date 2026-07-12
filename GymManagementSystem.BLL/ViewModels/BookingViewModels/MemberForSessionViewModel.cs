using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.BookingViewModels
{
    public class MemberForSessionViewModel
    {
        public int MemberId { get; set; }
        public int SessionId { get; set; }
        public string MemberName { get; set; } = default!;
        public string BookingDate { get; set; } = default!;
        public bool IsAttended { get; set; } = false;
    }
}
