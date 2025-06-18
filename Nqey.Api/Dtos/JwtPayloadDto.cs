namespace Nqey.Api.Dtos
{
    public class JwtPayloadDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public long Exp { get; set; }
        public string Iss { get; set; }
        public string Aud { get; set; }
    }
}
