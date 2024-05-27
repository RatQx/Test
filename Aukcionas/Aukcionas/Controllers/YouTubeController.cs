using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.AspNetCore.Mvc;
using Aukcionas.Models;

namespace Aukcionas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YouTubeController : ControllerBase
    {
        private static readonly string[] Scopes = { YouTubeService.Scope.Youtube };
        private static readonly string ApplicationName = "YourApplicationName";
        private static readonly string CredentialsFilePath = "path/to/credentials.json"; // Path to your credentials file
        private static readonly string TokenFolderPath = "path/to/tokenFolder"; // Path to store token file

        [HttpGet("authenticate")]
        public async Task<IActionResult> Authenticate()
        {
            UserCredential credential;

            using (var stream = new FileStream(CredentialsFilePath, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(TokenFolderPath, true)
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName
            });

            // Make authenticated requests to YouTube Data API
            // For example, create a live broadcast
            int auctionId = 123;
            await CreateLiveBroadcast(youtubeService, auctionId);

            return Ok("Authentication successful");
        }

        private async Task CreateLiveBroadcast(YouTubeService youtubeService, int auctionId)
        {
            try
            {
                var liveBroadcast = new Google.Apis.YouTube.v3.Data.LiveBroadcast
                {
                    Snippet = new Google.Apis.YouTube.v3.Data.LiveBroadcastSnippet
                    {
                        Title = $"Auction {auctionId} Live Stream", // Include auction ID in title
                        ScheduledStartTime = DateTime.UtcNow.AddMinutes(5) // Schedule the live stream 5 minutes from now
                    },
                    Status = new Google.Apis.YouTube.v3.Data.LiveBroadcastStatus
                    {
                        PrivacyStatus = "public" // Set privacy status to public
                    }
                };

                var request = youtubeService.LiveBroadcasts.Insert(liveBroadcast, "snippet,status");
                var response = await request.ExecuteAsync();

                var broadcastId = response.Id;
                Console.WriteLine($"Live broadcast created for Auction {auctionId} with ID: {broadcastId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating live broadcast for Auction {auctionId}: {ex.Message}");
            }
        }
    }
}
