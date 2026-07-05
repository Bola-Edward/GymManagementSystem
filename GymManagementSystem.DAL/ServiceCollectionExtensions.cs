using GymManagementSystem.DAL.Interceptors;
using GymManagementSystem.DAL.Repositories;
using GymManagementSystem.DAL.Repositories.Classes;
using GymManagementSystem.DAL.Repositories.Interfaces;
using GymManagementSystem.Data.Contexts;
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

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
