
namespace GymManagementSystem.DAL.Models.ValueObjects
{
    public class Address
    {
        public string Street { get; set; } = default!;
        public string City { get; set; } = default!;
        public int BuildingNumber { get; set; }

    }
}
