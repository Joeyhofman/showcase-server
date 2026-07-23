namespace API.Middleware;

using Serilog.Context;

public class AuthenticationAuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationAuditMiddleware> _logger;


    public AuthenticationAuditMiddleware(
        RequestDelegate next,
        ILogger<AuthenticationAuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }


    public async Task Invoke(HttpContext context)
    {
        var path = context.Request.Path;


        if (path.StartsWithSegments("/register") ||
            path.StartsWithSegments("/login") ||
            path.StartsWithSegments("/forgotPassword") ||
            path.StartsWithSegments("/resetPassword"))
        {
            var ip =
                context.Connection.RemoteIpAddress?.ToString();


            using(LogContext.PushProperty("ClientIp", ip))
            using(LogContext.PushProperty("AuthEndpoint", path))
            {
                await _next(context);


                _logger.LogInformation(
                    "Authentication endpoint executed. Endpoint {Endpoint} returned {StatusCode}",
                    path,
                    context.Response.StatusCode);
            }

            return;
        }


        await _next(context);
    }
}