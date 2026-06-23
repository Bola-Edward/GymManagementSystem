using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Member : User
    {
        public string? Photo { get; set; }
        public DateTime JoinDate { get; set; }

        public HealthRecord HealthRecord { get; set; } = null!;
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<MemberShip> Memberships { get; set; } = new List<MemberShip>();
    }
}
