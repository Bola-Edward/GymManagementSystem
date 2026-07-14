using GymManagementSystem.BLL.Attachments;
using GymManagementSystem.BLL.Mapping;
using GymManagementSystem.BLL.Services.Classes;
using GymManagementSystem.BLL.Services.Interfaces;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace GymManagementSystem.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {

            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<ITrainerService, TrainerService>();
            services.AddScoped<ISessionService, SessionService>();
            services.AddScoped<IMembershipService, MembershipService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IAttachmentService>(provider =>
            {
                var env = provider.GetRequiredService<IHostEnvironment>();
                var attachmentsPath = Path.Combine(env.ContentRootPath, "wwwroot", "Attachments");
                return new AttachmentService(attachmentsPath);
            });

            services.AddAutoMapper(cfg => cfg.AddMaps(typeof(MemberMappingProfile).Assembly));

            return services;
        }
    }
}
