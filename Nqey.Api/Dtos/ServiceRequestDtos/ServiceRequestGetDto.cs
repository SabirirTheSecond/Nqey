using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ServiceRequestDtos
{
    public class ServiceRequestGetDto
    {
        public string ServiceRequestId { get; set; }
        public string RequestName { get; set; }
        public string Description { get; set; }
        public int? ProviderUserId { get; set; }
        public ProviderPublicGetDto? Provider { get; set; }
        public ServiceRequestStatus ServiceRequestStatus { get; set; }
    }
}
