using GymManagementSystem.DAL.Enums;
using GymManagementSystem.DAL.Models.ValueObjects;

namespace GymManagementSystem.DAL.Models
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public DateOnly DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; } = null!;
    }
}
