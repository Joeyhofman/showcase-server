using Application.Exceptions;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace API.ExceptionHandlers
{
    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> _logger;

        public ExceptionHandler(ILogger<ExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            if(exception is ValidationException validationException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "Validation Error",
                    Type = "http://localhost:3000/",
                    Detail = exception.Message
                };
                problemDetails.Extensions.Add("errors", validationException.Errors);
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
                _logger.LogError($"{nameof(validationException)} occurred at {DateTime.UtcNow}");
                return true;
            }
            else if (exception.InnerException is DomainException domainException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "logic error",
                    Type = "http://localhost:3000/",
                    Detail = domainException.Message
                };

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
                _logger.LogError($"{nameof(domainException)} occurred at {DateTime.UtcNow}");
                return true;
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Internal Server Errro",
                    Type = "http://localhost:3000/",
                    Detail = "Something went wrong!"
                };

                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await httpContext.Response.WriteAsJsonAsync(problemDetails);
                _logger.LogError($"{nameof(exception)} with exception message {exception.StackTrace} occurred at {DateTime.UtcNow}");
                return true;
            }
        }
    }
}
