using GymManagementSystem.BusinessLogic.Services.Classes;
using GymManagementSystem.BusinessLogic.Services.Interfaces;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IMemberService, MemberService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ITrainerService, TrainerService>();


            return services;
        }
    }
}
