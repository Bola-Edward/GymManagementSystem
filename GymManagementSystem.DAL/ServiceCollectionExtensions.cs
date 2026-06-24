using GymManagementSystem.DAL.Interceptors;
using GymManagementSystem.Data.Contexts;
using GymManagementSystem.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.DAL
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGymDataAccess(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<AuditColumnsInterceptor>();

            services.AddDbContext<GymDbContext>((IServiceProvider sp, DbContextOptionsBuilder options) =>
            {
                options.UseSqlServer(connectionString);
                options.AddInterceptors(sp.GetRequiredService<AuditColumnsInterceptor>());
            });

            services.AddScoped<IPlanRepository, PlanRepository>();

            return services;
        }
    }
}
