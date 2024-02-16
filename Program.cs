using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Gamehub.Api.Data;
using Gamehub.Api.Services;
using Gamehub.Api.Endpoints;
using Gamehub.Api.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
builder.Services.AddScoped<WebhookHandler>(serviceProvider =>
{
    var userService = serviceProvider.GetRequiredService<IUserService>();
    var webhookSecret = builder.Configuration["WebhookSecret"];
    return new WebhookHandler(userService, webhookSecret);
});


var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

app.MapGet("/", () => "Hello World!");
app.MapUserEndpoints();

app.Run();
