using Accountant_JWT_implementation.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Accountant_JWT_implementation.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly JwtSettings _settings;

        public WeatherController(JwtSettings settings)
        {
            _settings = settings;
        }

        [HttpGet]
        public IActionResult GetWeather([FromQuery] string bearerToken)
        {
            if (string.IsNullOrEmpty(bearerToken))
            {
                return BadRequest(new { Error = "Bearer token is required as a query parameter." });
            }

            // Validate token manually
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.SecretKey);

            try
            {
                tokenHandler.ValidateToken(bearerToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _settings.Issuer,
                    ValidAudience = _settings.Audience,
                    ValidateLifetime = true,
                }, out var validatedToken);

                // Token is valid
                return Ok(new { Weather = "Sunny", Temperature = "25°C" });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }
    }
}
