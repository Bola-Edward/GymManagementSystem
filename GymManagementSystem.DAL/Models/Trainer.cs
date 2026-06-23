using GymManagementSystem.DAL.Enums;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer : User
    {
        public Speciality Speciality { get; set; } = default!;
        public DateTime HireDate { get; set; }

    }
}
