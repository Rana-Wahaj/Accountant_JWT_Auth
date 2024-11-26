using Accountant_JWT_implementation.Model;
using Accountant_JWT_implementation.Services;
using Microsoft.AspNetCore.Mvc;

namespace Accountant_JWT_implementation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtTokenGenerator _tokenGenerator;

        public AuthController(JwtSettings jwtSettings)
        {
            _tokenGenerator = new JwtTokenGenerator(jwtSettings);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            // Simple username/password check (replace with real validation logic)
            if (user.Username == "admin" && user.Password == "password")
            {
                var token = _tokenGenerator.GenerateToken(user.Username);
                return Ok(token);
            }

            return Unauthorized("Invalid credentials");
        }
    }
}
