using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Nqey.Domain.Common;
using Nqey.Domain;

namespace Nqey.Services.Services
{
    public class JwtService
    {
        private readonly JwtSettings _jwtSettings;

        public JwtService(IOptions<JwtSettings> options)
        {
            _jwtSettings = options.Value;
        }

        public GeneratedToken GenerateToken(User user)
        {
            var claims = new List<Claim>
        {
            new Claim("userId", user.UserId.ToString()),
            new Claim("userName", user.UserName),
            new Claim("role", user.UserRole.ToString())
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);
            
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audiences.First(),
                claims: claims,
                expires: expires,
                signingCredentials: creds);

            return new GeneratedToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expires,

            }
            ;
        }
    }

}
