using Accountant_JWT_implementation.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;



namespace Accountant_JWT_implementation.Services
{
    
        public class JwtTokenGenerator
        {
            private readonly JwtSettings _settings;

            public JwtTokenGenerator(JwtSettings settings)
            {
                _settings = settings;
            }

            public LoginResponse GenerateToken(string username)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var expiration = DateTime.UtcNow.AddMinutes(_settings.ExpirationMinutes);

                var token = new JwtSecurityToken(
                    issuer: _settings.Issuer,
                    audience: _settings.Audience,
                    claims: claims,
                    expires: expiration,
                    signingCredentials: credentials
                );

                return new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = expiration
                };
            }
        }
    

}
