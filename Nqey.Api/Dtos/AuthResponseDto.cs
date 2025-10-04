using Nqey.Api.Dtos.ClientDtos;
using Nqey.Api.Dtos.JwtPayloadDtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;

namespace Nqey.Api.Dtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public JwtPayloadDto Payload { get; set; }

        public ProviderAdminGetDto? Provider { get; set; }
        public ClientPublicGetDto? Client { get; set; }
    }
}
