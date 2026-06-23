using GymManagementSystem.DAL.Enums;

namespace GymManagementSystem.DAL.Models
{
    public class Trainer : User
    {
        public Speciality Speciality { get; set; } = default!;
        public DateTime HireDate { get; set; }
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}
