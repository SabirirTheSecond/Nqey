using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.Domain;

using Nqey.Services.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JwtService _jwtService;

        public AuthController(IUserRepository userRepository, JwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByUserNameAsync(loginDto.Username);

           

            if (user == null || !user.VerifyPassword(loginDto.Password))
                return Unauthorized(new ApiResponse<string>(false, "Invalid credentials", null));
            Console.WriteLine($"your userRole is : {user.UserRole}");
            if (!Enum.TryParse<Role>(loginDto.AppType, true, out var parsedRole))
            {
                return BadRequest(new ApiResponse<string>(false, "Invalid AppType", null));
            }

            if (user.UserRole != parsedRole)
            {
                return Unauthorized(new ApiResponse<string>(false, $"Your {user.UserRole} app version is not compatible", null));
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.MobilePhone, user.PhoneNumber),
                new Claim(ClaimTypes.Role, user.UserRole.ToString()),                                             
            
            };

            var identity = new ClaimsIdentity(claims);
            var token = _jwtService.GenerateToken(user);
            
            var payload = new JwtPayloadDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                Email= user.Email,
                PhoneNumber= user.PhoneNumber,
                Role = user.UserRole.ToString(),
                ProfileImagePath= user.ProfileImage?.ImagePath,
                Exp = new DateTimeOffset(token.Expiration).ToUnixTimeSeconds(),
                Iss = "Nqey",
                Aud = $"Nqey-{user.UserRole.ToString().ToLower()}"
            };
            var authResponse = new AuthResponseDto
            {
                Payload = payload,
                Token = token.Token
            };
            return Ok(new ApiResponse<AuthResponseDto>(true, "Login successful", authResponse));
        }
    }
}

