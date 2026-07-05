using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.MemberViewModels
{
    public class MemberDetailsViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? PhotoUrl { get; set; }
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string PLanName { get; set; } = null!;
        public string MembershipStartDate { get; set; } = null!;
        public string MembershipEndDate { get; set; } = null!;
    }
}
