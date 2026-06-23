namespace GymManagementSystem.DAL.Models
{
    public class Booking : BaseEntity
    {
        public DateTime Date { get; set; }
        public bool IsAttended { get; set; }
    }
}
