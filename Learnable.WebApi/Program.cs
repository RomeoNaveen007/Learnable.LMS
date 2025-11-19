using Learnable.Application.Features.Users.Commands.RegisterUser;
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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

// Caling the DI class of Infrastructure 
builder.Services.AddInfrastructureDb(builder.Configuration);

builder.Services.AddMediatR(cfg =>
               cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));

builder.Services.AddCors();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => // Configure the JWT bearer authentication options. This will specify how the JWT token should be validated and used for authentication.
        {
            var TokenKey = builder.Configuration["TokenKey"] // Get the secret key used to sign the JWT tokens from the configuration file. This key is used to validate the token's signature and ensure that it has not been tampered with.
                ?? throw new InvalidOperationException("TokenKey is not configured."); // If the TokenKey is not found in the configuration, throw an exception to indicate that the application cannot start without it. This is a safeguard to ensure that the application does not start with an invalid or missing key.
            options.TokenValidationParameters = new TokenValidationParameters // Create a new instance of the TokenValidationParameters class to specify the validation parameters for the JWT token.
            {
                ValidateIssuerSigningKey = true, // Enable validation of the token's signing key to ensure that it is valid and has not been tampered with. This is a critical security measure to prevent unauthorized access to the API.
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenKey)), // Set the signing key to the secret key obtained from the configuration file. This key is used to validate the token's signature.
                ValidateIssuer = false, // Disable validation of the token's issuer. This means that the token can be issued by any trusted authority and does not need to match a specific issuer. This is useful for applications that use multiple issuers or want to allow tokens issued by different authorities.
                ValidateAudience = false // Disable validation of the token's audience. This means that the token can be used by any audience and does not need to match a specific audience. This is useful for applications that want to allow tokens to be used by multiple audiences or do not require strict audience validation.
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
