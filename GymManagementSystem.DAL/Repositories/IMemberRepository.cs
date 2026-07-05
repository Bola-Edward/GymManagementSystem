using GymManagementSystem.DAL.Models;

namespace GymManagementSystem.DAL.Repositories
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<bool> IsEmailTakenAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        Task<bool> IsPhoneTakenAsync(string phone, CancellationToken cancellationToken = default);

        Task<Member?> GetWithMembershipsAsync(int id, CancellationToken cancellationToken = default);
    }
}