using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using KofCWSC.API.Models;

namespace KofCWSC.API.Controllers
{
    public class AddressRequestsController : Controller
    {
        //private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        const string JWTbaseUrl = "https://api.usps.com/oauth2/v3/token";


        public AddressRequestsController(IConfiguration configuration)
        {
            //_context = context;
            _configuration = configuration;

        }
        [HttpPost("ValidateAddress")]
        public async Task<IActionResult> ValidateAddress([FromBody] USPSAddress addressRequest) // string street, string city, string state, string zipCode)
        {
            // USPS API endpoint
            var endpoint = "https://api.usps.com/addresses/v3/address";

            // Replace these with your actual Consumer Key and Secret
            var consumerKey = _configuration["USPS:ConsumerKey"];
            var consumerSecret = _configuration["USPS:ConsumerSecret"];

            var jwtToken = await GetJwtTokenAsync(JWTbaseUrl, consumerKey, consumerSecret);
            Log.Information(jwtToken);
            var httpClient = new HttpClient();

            var payload = new
            {
                client_id = consumerKey,
                client_secret = consumerSecret,
                grant_type = "client_credentials"
            };

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);

            
            try
            {
                // Create query parameters (modify as per USPS API documentation)
                var queryParams = $"?streetAddress={Uri.EscapeDataString(addressRequest.Address.StreetAddress)}&city={Uri.EscapeDataString(addressRequest.Address.City)}&state={Uri.EscapeDataString(addressRequest.Address.State)}&ZIPCode={Uri.EscapeDataString(addressRequest.Address.ZIPCode)}";
                var requestUri = endpoint + queryParams;

                // Call USPS API
                var responseData = await httpClient.GetAsync(requestUri);

                if (responseData.IsSuccessStatusCode)
                {
                    var contentAddr = await responseData.Content.ReadAsStringAsync();
                    var desAddr = JsonSerializer.Deserialize<USPSAddress>(contentAddr, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    // Return API response (you can also deserialize to a model)
                    //ViewBag.USPSAddress = "Found This Address";
                    return Ok(desAddr);
                }
                else
                {
                    var myError = await responseData.Content.ReadAsStringAsync();
                    var myRetVal = new USPSAddress();
                    return Ok(myRetVal);
                    //return StatusCode((int)responseData.StatusCode, await responseData.Content.ReadAsStringAsync());
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                //return StatusCode(500, $"Internal server error: {ex.Message}");
                return NotFound($"Bad Input Address Format. 1 or more elements may be null");
            }
        }
        static async Task<string> GetJwtTokenAsync(string baseUrl, string clientId, string clientSecret)
        {
            using var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

            // Prepare the request payload
            var payload = new
            {
                client_id = clientId,
                client_secret = clientSecret,
                grant_type = "client_credentials"
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            // Send POST request to the authentication endpoint
            Log.Information(baseUrl + content.ToString());
            var response = await client.PostAsync(baseUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                dynamic jsonResponse = JsonSerializer.Deserialize<JwtResponse>(responseString);
                Log.Information(jsonResponse.access_token);
                return jsonResponse.access_token;
            }
            else
            {
                Log.Error($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                return null;
            }
        }
    }
}
