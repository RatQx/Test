using Aukcionas.Controllers;
using Aukcionas.Utils;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using static System.Net.WebRequestMethods;


namespace Aukcionas.Services
{
    public class DailycoService : IDailycoService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuctionController> _logger;
        public DailycoService(HttpClient httpClient, IConfiguration configuration, ILogger<AuctionController> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<string> StartLivestream()
        {
            var apiKey = _configuration["Dailyco:ApiKey"];
            var url = _configuration["Dailyco:ApiUrl"];
            var expirationTime = DateTimeOffset.UtcNow.AddHours(1).ToUnixTimeSeconds();

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                var createRoomBody = new
                {
                    properties = new
                    {
                        start_video_off = true,
                        start_audio_off = true,
                        enable_advanced_chat = true,
                        max_participants = 20,
                        exp = expirationTime
                    }
                };
                string createRoomJson = Newtonsoft.Json.JsonConvert.SerializeObject(createRoomBody);
                var createRoomRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Content = new StringContent(createRoomJson, Encoding.UTF8, "application/json")
                };
                HttpResponseMessage createRoomResponse = await client.SendAsync(createRoomRequest);
                if (createRoomResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Room created successfully.");
                    string roomData = await createRoomResponse.Content.ReadAsStringAsync();
                    dynamic roomInfo = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(roomData);
                    string roomUrl = roomInfo.url;
                    string livestreamUrl = roomUrl;
                    var livestreamRequest = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = new Uri(livestreamUrl)
                    };
                    HttpResponseMessage livestreamResponse = await client.SendAsync(livestreamRequest);
                    if (livestreamResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Livestream started successfully.");
                        return livestreamUrl;
                    }
                    else
                    {
                        Console.WriteLine("Error starting livestream. Status code: " + livestreamResponse.StatusCode);
                        string errorContent = await livestreamResponse.Content.ReadAsStringAsync();
                        Console.WriteLine("Error content: " + errorContent);
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine("Error creating room. Status code: " + createRoomResponse.StatusCode);
                    string errorContent = await createRoomResponse.Content.ReadAsStringAsync();
                    Console.WriteLine("Error content: " + errorContent);
                    return null;
                }
            }

        }
        public async Task StopLivestream(string auctionid,string roomid)
        {
            var apiKey = _configuration["Dailyco:ApiKey"];
            var url = _configuration["Dailyco:ApiUrl"];
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + apiKey);
                string stopLivestreamUrl = $"{url}/{roomid}/live-streaming/stop";
                var stopLivestreamRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(stopLivestreamUrl)
                };
                HttpResponseMessage stopLivestreamResponse = await client.SendAsync(stopLivestreamRequest);
                if (!stopLivestreamResponse.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to stop livestream. Please try again later.");
                }
            }
        }
    }
}
