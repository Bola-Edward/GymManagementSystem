using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Models
{
    public class Member : User
    {
        public string? Photo { get; set; }
        public DateTime JoinDate { get; set; }


    }
}
