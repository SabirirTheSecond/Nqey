using Nqey.Domain;

namespace Nqey.Api.Dtos.JwtPayloadDtos
{
    public class JwtPayloadDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ProfileImagePath { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public long Exp { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
