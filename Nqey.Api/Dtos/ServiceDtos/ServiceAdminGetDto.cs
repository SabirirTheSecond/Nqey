using Nqey.Api.Dtos.ProviderDtos;

namespace Nqey.Api.Dtos.ServiceDtos
{
    public class ServiceAdminGetDto
    {
        public int ServiceId { get; set; }
        public string NameEn { get; set; }
        public string NameFr { get; set; }
        public string NameAr { get; set; }
        public ProfileImageDtos.ProfileImageGetDto? Image { get; set; }
        public List<ProviderPublicGetDto>? Providers { get; set; }

    }
}
