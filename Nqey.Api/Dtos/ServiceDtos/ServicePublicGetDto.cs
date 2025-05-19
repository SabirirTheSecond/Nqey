using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;
using Nqey.Domain.Common;
namespace Nqey.Api.Dtos.ServiceDtos
{
    public class ServicePublicGetDto
    {
        public int ServiceId { get; set; }
        public string NameEn { get; set; }
        public string NameFr { get; set; }
        public string NameAr { get; set; }
        public Image? Image { get; set; }
        public List<ProviderPublicGetDto>? Providers { get; set; }

    }
}
