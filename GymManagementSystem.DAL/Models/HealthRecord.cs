using GymManagementSystem.DAL.Enums;

namespace GymManagementSystem.DAL.Models
{
    public class HealthRecord : BaseEntity
    {
        public decimal Height { get; set; }
        public decimal Weight { get; set; }
        public BloodType BloodType { get; set; }
        public string? Notes { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

    }
}
