using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Common.Email;
using Learnable.Infrastructure.Implementations.Repositories;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Implementations.Services.Internal;
using Learnable.Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnable.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDb(this IServiceCollection services, IConfiguration configuration)
        {
            // Get connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("LearnableDatabase");

            // Register DbContext with the connection string
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Bind SmtpSettings from appsettings
            services.Configure<SmtpSetting>(configuration.GetSection("Smtp"));

            // EmailService Register 
            services.AddScoped<IEmailService, EmailService>();

            // Token Registration
            services.AddScoped<ITokenService, TokenService>();

            //  GenericRepository Registration 
            services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepository<>));

            // UnitOfWork Registration
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // PasswordService Registration 
            services.AddScoped<IPasswordService, PasswordService>();

            // UserRepository Registration
            services.AddScoped<IUserRepository, UserRepository>();


            return services;
        }

    }
}
