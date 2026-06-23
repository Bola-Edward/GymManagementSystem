using GymManagementSystem.DAL.Enums;

namespace GymManagementSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public BloodType BloodType { get; set; }

    }
}
