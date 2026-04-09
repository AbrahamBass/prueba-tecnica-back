using Domain.Repositories;
using Infrastructure.Persistence.Db;
using Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;


namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<MyDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IPatientRepository, PatientRepository>();

            return services;
        }
    }
}
