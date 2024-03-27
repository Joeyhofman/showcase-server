using System;
using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Security.Claims;
using API;
using API.ExceptionHandlers;
using API.Hubs;
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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddDbContext<ApplicationDBContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("Default")));
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



builder.Services.AddAuthorization();


builder.Services.AddIdentityApiEndpoints<Domain.Entities.User>(options =>
{
    options.Password.RequiredLength = 8;

})
    .AddRoles<IdentityRole<Guid>>()
    .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
    .AddEntityFrameworkStores<ApplicationDBContext>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapIdentityApi<Domain.Entities.User>();


app.MapPost("/logout", async (SignInManager<Domain.Entities.User> signInManager) =>
{
    await signInManager.SignOutAsync().ConfigureAwait(false);
    return Results.Ok();
}).RequireAuthorization();


app.MapGet("/time", () =>
{
    return Results.Ok(new { Time = DateTime.Now });
});


app.UseExceptionHandler(_ => { });


app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

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
