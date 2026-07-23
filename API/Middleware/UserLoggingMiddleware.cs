using System.Security.Claims;
using Serilog.Context;

namespace API.Middleware;

public class UserLoggingMiddleware
{
    private readonly RequestDelegate _next;


    public UserLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {

        var userId =
            context.User.FindFirst(
                ClaimTypes.NameIdentifier)
                ?.Value;


        using (
          LogContext.PushProperty(
             "UserId",
             userId ?? "Anonymous"))
        {
            await _next(context);
        }
    }
}