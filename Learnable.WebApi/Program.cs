using Learnable.Application.Features.Account.Commands.RegisterUser;
using Learnable.Application.Interfaces.Repositories.Generic;
using Learnable.Application.Interfaces.Services;
using Learnable.Infrastructure;
using Learnable.Infrastructure.Persistence.Data;
using Learnable.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Caling the DI class of Infrastructure 
builder.Services.AddInfrastructureDb(builder.Configuration);

builder.Services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

builder.Services.AddCors();


builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var TokenKey = builder.Configuration["TokenKey"]
            ?? throw new InvalidOperationException("TokenKey is not configured.");

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };

        //  CUSTOM 401 HANDLING
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                // Prevent default behavior
                context.HandleResponse();

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var response = JsonSerializer.Serialize(new
                {
                    message = "Unauthorized (Invalid or missing JWT)"
                });

                return context.Response.WriteAsync(response);
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(policy =>
    policy.AllowAnyHeader()
          .AllowAnyMethod()
          .WithOrigins("http://localhost:4200", "https://localhost:4200"));

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

    await db.Database.MigrateAsync();
    await Seed.SeedUser(db, unitOfWork, passwordService);
}

app.Run();
