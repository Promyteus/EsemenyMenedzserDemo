using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EsemenyMenedzser.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred.");

            var response = new ProblemDetails();

            switch (exception)
            {
                case ValidationException validationEx:
                    _logger.LogWarning("Validation error occurred: {ValidationMessage}", validationEx.Message);
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Validation failed";
                    response.Detail = validationEx.Message;
                    response.Extensions["errors"] = validationEx.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(
                            g => g.Key,
                            g => g.Select(e => e.ErrorMessage).ToArray()
                        );
                    break;

                case ArgumentNullException argNullEx:
                    _logger.LogWarning("Argument null exception: {ArgNullMessage}", argNullEx.Message);
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Title = "Invalid argument";
                    response.Detail = argNullEx.Message;
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    _logger.LogWarning("Resource not found: {NotFoundMessage}", keyNotFoundEx.Message);
                    response.Status = StatusCodes.Status404NotFound;
                    response.Title = "Not found";
                    response.Detail = keyNotFoundEx.Message;
                    break;

                default:
                    _logger.LogError("Unhandled exception type: {ExceptionType}", exception.GetType().Name);
                    response.Status = StatusCodes.Status500InternalServerError;
                    response.Title = "Internal server error";
                    response.Detail = "An unexpected error occurred. Please try again later.";
                    break;
            }

            httpContext.Response.StatusCode = response.Status.Value;
            httpContext.Response.ContentType = "application/json";

            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
