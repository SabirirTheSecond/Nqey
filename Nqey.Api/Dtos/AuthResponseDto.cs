namespace Nqey.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public JwtPayloadDto Payload { get; set; }
    }
}
