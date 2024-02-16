using Gamehub.Api.Services;
using Gamehub.Api.Models;
using Gamehub.Api.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace Gamehub.Api.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/{clerkId}", async (string clerkId, IUserService userService) =>
            {
                var user = await userService.GetUserByClerkIdAsync(clerkId);
                return user is not null ? Results.Ok(user) : Results.NotFound();
            });

            app.MapPut("/api/users/{clerkId}", async (string clerkId, User updatedUserStats, IUserService userService) =>
            {
                try
                {
                    var user = await userService.UpdateUserByClerkIdAsync(clerkId, updatedUserStats);
                    return Results.Ok(user);
                }
                catch (KeyNotFoundException)
                {
                    return Results.NotFound();
                }
            });

            app.MapPost("/api/users", async (User newUser, IUserService userService) =>
            {
                var user = await userService.CreateUserAsync(newUser);
                return Results.Created($"/api/users/{user.Id}", user);
            });

            app.MapGet("/api/users", async (IUserService userService) =>
            {
                var users = await userService.GetAllUsersAsync();
                return Results.Ok(users);
            });

            app.MapPost("/api/users/webhook", async (HttpContext context, WebhookHandler handler, IUserService userService) =>
            {
                return await handler.HandleWebhook(context);
            });

            app.MapGet("/api/users/leaderboard", async (ILeaderboardService leaderboardService) =>
            {
                var leaderboardData = await leaderboardService.GetLeaderboardDataAsync();
                return Results.Ok(leaderboardData);
            });
        }
    }
}