using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.RateLimiting;
using API;
using API.ExceptionHandlers;
using API.Hubs;
using API.Middleware;
using Application.Behavours;
using Application.ContactForm.Commands.SendMessage;
using Application.Validators;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;



var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, loggerConfig) => loggerConfig.ReadFrom.Configuration(context.Configuration));

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DIAGRAM_CONNECTION_STRING")));
builder.Services.AddSignalR();

//infrastructure
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IInvitationRepository, InvitationRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDataflowDiagramRepository, DataflowDiagramRepository>();

// Add services to the container.
builder.Services.AddMediatR(cfg =>
     cfg.RegisterServicesFromAssembly(typeof(SendMessageCommand).Assembly));


builder.Services.AddScoped<IValidator<SendMessageCommand>, SendMessageCommandValidator>();
builder.Services.AddValidatorsFromAssembly(typeof(SendMessageCommand).Assembly);

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto;
});

var disableRateLimiting =
    Environment.GetEnvironmentVariable("DISABLE_RATE_LIMITING") == "true";

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = 429;

        await context.HttpContext.Response.WriteAsJsonAsync(
            new
            {
                error = "Too many requests. Please try again later."
            },
            cancellationToken: token);
    };

    options.AddPolicy("api", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            });
    });


    options.AddPolicy("login", httpContext =>
    {
        if (disableRateLimiting)
        {
            return RateLimitPartition.GetNoLimiter("disabled");
        }

        var ip = httpContext.Connection.RemoteIpAddress?.ToString();

        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip ?? "unknown",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(5),
                QueueLimit = 0
            });
    });
});

builder.Services.AddAuthorization();


builder.Services.AddIdentityApiEndpoints<Domain.Entities.User>(options =>
{
    options.Password.RequiredLength = 12;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Lockout.AllowedForNewUsers = true;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);

})
    .AddRoles<IdentityRole<Guid>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddEntityFrameworkStores<ApplicationDBContext>();

builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromHours(1);
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000", "https://diagramming.com", "https://acceptance.diagramming.com", "https://test.diagramming.com")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
        });
});


builder.Services.AddScoped<IContactEmailService, ContactEmailService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseForwardedHeaders();
app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<Domain.Entities.User>().RequireRateLimiting("login");


app.MapPost("/logout", async (SignInManager<Domain.Entities.User> signInManager) =>
{
    await signInManager.SignOutAsync().ConfigureAwait(false);
    return Results.Ok();
}).RequireAuthorization();


app.MapGet("/time", () =>
{
    return Results.Ok(new { Time = DateTime.Now });
});



app.UseSerilogRequestLogging();
app.UseMiddleware<RequestContextMiddleware>();
app.UseMiddleware<UserLoggingMiddleware>();
app.UseMiddleware<AuthenticationAuditMiddleware>();

app.UseExceptionHandler(_ => { });



app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");


app.UseAuthentication();
app.UseAuthorization();


app.MapHub<DiagramEditingHub>("/hubs/diagramediting");

app.MapControllers();


app.ApplyMigrations();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
    if(!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
    var user = await userManager.FindByEmailAsync("admin@admin.nl");

    if (user != null)
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
    else
    {
        var newUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "admin@admin.nl",
            UserName = "admin@admin.nl"
        };
        var result = await userManager.CreateAsync(newUser, "Infra123#");

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(newUser, "Admin");
        }
        else
        {
            Console.WriteLine("could not create user");
        }
    }
}


app.Run();
