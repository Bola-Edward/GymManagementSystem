using Microsoft.AspNetCore.Identity;


namespace GymManagementSystem.DAL.Data.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string DisplayName { get; set; } = null!;
    }
}
