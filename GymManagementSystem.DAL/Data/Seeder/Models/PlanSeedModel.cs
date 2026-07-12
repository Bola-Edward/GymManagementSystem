using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL.Data.Seeder.Models
{
    public class PlanSeedModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int DurationDays { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
