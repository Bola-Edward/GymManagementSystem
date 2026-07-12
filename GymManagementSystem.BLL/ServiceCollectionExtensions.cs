using GymManagementSystem.BLL.Mapping;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace GymManagementSystem.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IMemberService, MemberService>();
            IServiceCollection serviceCollection = services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IBookingService, BookingService>();

            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MemberMappingProfile).Assembly));

            return services;
        }
    }
}
