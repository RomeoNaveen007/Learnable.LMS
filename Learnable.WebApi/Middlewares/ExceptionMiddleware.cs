using FluentValidation;
using Learnable.Domain.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Learnable.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ValidationException ex)
            {
                _logger.LogWarning("Validation failed: {Errors}", ex.Errors);

                var response = new ApiException(
                    (int)HttpStatusCode.BadRequest,
                    "Validation failed",
                    string.Join("; ", ex.Errors.Select(e => e.ErrorMessage))
                );

                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    errors = ex.Errors
                                .GroupBy(e => e.PropertyName)
                                .ToDictionary(
                                    g => g.Key,
                                    g => g.Select(e => e.ErrorMessage).ToArray()
                                )
                }));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                var response = new ApiException(
                    (int)HttpStatusCode.InternalServerError,
                    ex.Message,
                    ex.StackTrace
                );

                context.Response.StatusCode = response.StatusCode;
                context.Response.ContentType = "application/json";

                // Dev vs Prod JSON formatting
                await context.Response.WriteAsync(
                    _env.IsDevelopment()
                    ? JsonSerializer.Serialize(new
                    {
                        response.StatusCode,
                        response.Message,
                        response.Details
                    })
                    : JsonSerializer.Serialize(new
                    {
                        message = "An unexpected error occurred."
                    })
                );
            }
        }
    }
}
