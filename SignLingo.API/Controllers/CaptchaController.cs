using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SignLingo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CaptchaController : ControllerBase
    {
        private readonly string _secretKey = "6LdAjeUpAAAAANa_tNMAlJjqkZC8KT8L7gWDrIcx"; // Reemplaza 'YOUR_SECRET_KEY' con tu clave secreta

        [HttpPost("verify-captcha")]
        public async Task<IActionResult> VerifyCaptcha([FromBody] CaptchaRequest request)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(
                    $"https://www.google.com/recaptcha/api/siteverify?secret={_secretKey}&response={request.Token}",
                    null);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JObject.Parse(jsonResponse);

                if (result.Value<bool>("success"))
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return BadRequest(new { success = false });
                }
            }
        }
    }
}

public class CaptchaRequest
{
    public string Token { get; set; }
}