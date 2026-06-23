using GymManagementSystem.Models;

namespace GymManagementSystem.DAL.Models
{
    public class MemberShip : BaseEntity
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
