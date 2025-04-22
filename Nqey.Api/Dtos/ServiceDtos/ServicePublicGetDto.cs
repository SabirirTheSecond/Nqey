using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;
namespace Nqey.Api.Dtos.ServiceDtos
{
    public class ServicePublicGetDto
    {
        public int ServiceId { get; set; }
        public string Name { get; set; }
        public List<ProviderPublicGetDto>? Providers { get; set; }

    }
}
