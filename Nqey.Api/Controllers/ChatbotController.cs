using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatbotController: Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _ngrokUrl;
        public ChatbotController(IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory, IConfiguration configuration) 
        {
            _contextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _ngrokUrl = configuration["MLApi:NgrokUrl"];
        
        }

        public class ChatRequest
        {
            public string Message { get; set; }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessageToChatBot([FromBody] ChatRequest req)
        {
            var userId = int.Parse(User.FindFirstValue("userId"));

            string jwt = _contextAccessor.HttpContext.Request.Headers.Authorization
                                        .ToString()
                                        .Replace("Bearer ",""); // tremplacer les occurences Bearer token... b ""
                                                                // twelli ghir token
            
            var rasaPayload = new
            {
                sender = userId,
                message = req.Message,
                metadata = new { jwt }
            };
            var client = _httpClientFactory.CreateClient();
            var json = JsonSerializer.Serialize(rasaPayload);
            var content = new StringContent(json, Encoding.UTF8, "application/json"
                );
            var response= await client.PostAsync($"{_ngrokUrl}/webhooks/rest/webhook", content);
            var reply = await response.Content.ReadAsStringAsync();
            return Content(reply, "application/json");
        }

    }
}
