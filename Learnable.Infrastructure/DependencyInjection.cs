using FluentValidation;
using Learnable.Application.Common.Behaviors;
using Learnable.Application.Features.Account.Commands.RegisterTeacher;
using Learnable.Application.Features.Users.Queries.LoginUser;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Domain.Common.Email;
using Learnable.Infrastructure.Implementations.Repositories;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Implementations.Services.Internal;
using Learnable.Infrastructure.Persistence.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learnable.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDb(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Connection string
            var connectionString = configuration.GetConnectionString("LearnableDatabase");

            // DB Context
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Email SMTP settings
            services.Configure<SmtpSetting>(configuration.GetSection("Smtp"));
            services.AddScoped<IEmailService, EmailService>();

            // Token & Password Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            // Generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // User Repository
            services.AddScoped<IUserRepository, UserRepository>();

            // Teacher Repository
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            // Class Repository
            services.AddScoped<IClassRepository, ClassRepository>();

            // Pipeline behaviors
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionBehavior<,>));

            // FluentValidation
            services.AddValidatorsFromAssembly(typeof(LoginQueryValidator).Assembly);
           
            // MediatR Handlers
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            });
            services.AddTransient<Seed>();

            return services;
        }
    }
}
