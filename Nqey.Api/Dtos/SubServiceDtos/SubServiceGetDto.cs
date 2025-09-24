using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;

namespace Nqey.Api.Dtos.SubServiceDtos
{
    public class SubServiceGetDto
    {
        public int SubServiceId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double? Cost { get; set; }
        public string? Unity { get; set; }
        public int ProviderUserId { get; set; }
        //public ProviderPublicGetDto Provider { get; set; }
    }
}
