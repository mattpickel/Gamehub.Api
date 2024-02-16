using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Svix;
using System.Net;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Gamehub.Api.Models;
using Gamehub.Api.Services;

namespace Gamehub.Api.Handlers
{
    public class WebhookHandler
    {
        private readonly IUserService _userService;
        private readonly string _webhookSecret;

        public WebhookHandler(IUserService userService, string webhookSecret)
        {
            _userService = userService;
            _webhookSecret = webhookSecret;
        }

        public async Task<IResult> HandleWebhook(HttpContext context)
        {
            var req = context.Request;
            Console.WriteLine("Handling webhook...");

            // Check if the 'Signing Secret' was provided
            if (string.IsNullOrEmpty(_webhookSecret))
            {
                Console.WriteLine("Webhook secret is not configured.");
                return Results.BadRequest("Webhook secret is not configured.");
            }

            // Grab the headers
            req.Headers.TryGetValue("svix-id", out StringValues svixId);
            req.Headers.TryGetValue("svix-timestamp", out StringValues svixTimestamp);
            req.Headers.TryGetValue("svix-signature", out StringValues svixSignature);

            var headers = new WebHeaderCollection();
            headers.Set("svix-id", req.Headers["svix-id"]);
            headers.Set("svix-timestamp", req.Headers["svix-timestamp"]);
            headers.Set("svix-signature", req.Headers["svix-signature"]);

            // If there are missing Svix headers, error out
            if (string.IsNullOrEmpty(svixId) || string.IsNullOrEmpty(svixTimestamp) || string.IsNullOrEmpty(svixSignature))
            {
                return Results.BadRequest("Error occurred -- no svix headers");
            }

            // Read the payload body
            string payload = await new StreamReader(req.Body).ReadToEndAsync();


            // Verify the signature and process the webhook...
            try
            {
                var wh = new Webhook(_webhookSecret);
                wh.Verify(payload, headers); // Adjusted for Svix API
                Console.WriteLine("Webhook verified successfully.");

                var webhookData = JsonSerializer.Deserialize<WebhookPayload>(payload);
                Console.WriteLine(webhookData.data.username);
                if (webhookData?.data?.username == null)
                {
                    Console.WriteLine("Webhook payload does not contain a username.");
                    return Results.BadRequest("Missing username in webhook data.");
                }

                if (webhookData?.data?.id == null)
                {
                    Console.WriteLine("Webhook payload does not contain a clerkId.");
                    return Results.BadRequest("Missing clerkId in webhook data.");
                }

                // Assuming your CreateUserAsync method expects a user model
                var newUser = new User
                {
                    Username = webhookData.data.username,
                    ClerkId = webhookData.data.id
                };

                var createdUser = await _userService.CreateUserAsync(newUser);

                Console.WriteLine($"New user created with username: {createdUser.Username}");
                return Results.Created($"/api/users/{createdUser.Id}", createdUser);
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                Console.WriteLine($"Webhook verification failed: {ex.Message}");
                return Results.BadRequest($"Webhook verification failed: {ex.Message}");
            }
        }
    }
}