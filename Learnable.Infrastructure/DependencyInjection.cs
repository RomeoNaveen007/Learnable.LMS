using FluentValidation;
using Learnable.Application;
using Learnable.Application.Common.Behaviors;
using Learnable.Application.Features.Account.Commands.RegisterTeacher;
using Learnable.Application.Features.Account.Commands.RegisterUser;
using Learnable.Application.Features.Account.Queries.LoginUser;
using Learnable.Application.Features.Asset.Commands.AddAsset;
using Learnable.Application.Features.Asset.Queries.GetAssetByID_;
using Learnable.Application.Features.Class.Commands.AddClass;
using Learnable.Application.Features.Class.Commands.DeleteClass;
using Learnable.Application.Features.Class.Commands.UpdateClass;
using Learnable.Application.Features.Class.Queries.GetById;
using Learnable.Application.Features.Class.Queries.GetByUnicName;
using Learnable.Application.Features.Exam.Commands.Create;
using Learnable.Application.Features.Exam.Commands.Delete;
using Learnable.Application.Interfaces.Repositories;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Application.Interfaces.Services.External;
using Learnable.Domain.Common.Email;
using Learnable.Infrastructure.Implementations.Repositories;
using Learnable.Infrastructure.Implementations.Repositories.Generic;
using Learnable.Infrastructure.Implementations.Services.External;
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

            //apiservices
            services.AddScoped<IAiApiService, AiApiService>();

            //explanation service
            services.AddScoped<IExplanationService, ExplanationService>();

            //exam ai service
            services.AddScoped<IExamAiApiService, ExamAiApiService>();

            // Generic repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // User Repository
            services.AddScoped<IUserRepository, UserRepository>();

            // Repository Repository
            services.AddScoped<IRepositoryRepository, RepositoryRepository>();

            //RequestNotificationRepository
            services.AddScoped<IRequestNotificationRepository, RequestNotificationRepository>();

            // Exam Repository
            services.AddScoped<IExamRepository, ExamRepository>();

            // ClassStudentRepository
            services.AddScoped<IClassStudentRepository, ClassStudentRepository>();

            // Teacher Repository
            services.AddScoped<ITeacherRepository, TeacherRepository>();

            // Class Repository
            services.AddScoped<IClassRepository, ClassRepository>();

            // Exam Repository
            services.AddScoped<IExamRepository,ExamRepository>();

            //marks repo 
            services.AddScoped<IMarksRepostiory, MarksRepository>();

            // Asset Repository
            services.AddScoped<IAssetReopsitory, AssetReopsitory>();

            services.AddTransient<Seed>();

            services.AddApplication();

            return services;
        }
    }
}
