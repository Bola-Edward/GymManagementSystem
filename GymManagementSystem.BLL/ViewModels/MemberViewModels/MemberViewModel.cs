using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.ViewModels.MemberViewModels
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string PhotoUrl { get; set; } = null!;
        public DateOnly JoinDate { get; set; }
        public string Gender { get; set; } = null!;
    }
}
