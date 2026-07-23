namespace API.Middleware;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Serilog.Context;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;


    public RequestContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }


    public async Task Invoke(HttpContext context)
    {
        var requestId = Guid.NewGuid().ToString();


        using (LogContext.PushProperty(
            "RequestId",
            requestId))
        {
            await _next(context);
        }
    }
}