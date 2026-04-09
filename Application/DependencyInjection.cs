using Application.DTOs.Patient;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IValidator<CreatePatientDto>, PatientDtoValidator>();
            services.AddScoped<IValidator<UpdatePatientDto>, PatientDtoValidator>();
            services.AddScoped<IPatientService, PatientService>();

            return services;
        }
    }
}
