using Learnable.Application.Features.Users.Commands.RegisterUser;
using Learnable.Infrastructure;
using Learnable.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddCors(policy =>
                policy.AddDefaultPolicy(options =>
                    options.AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins("http://localhost:4200", "https://localhost:4200")));

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

//app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
