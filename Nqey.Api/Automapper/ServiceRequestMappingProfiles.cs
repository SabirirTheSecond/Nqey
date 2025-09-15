using AutoMapper;
using Nqey.Api.Dtos.ServiceRequestDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ServiceRequestMappingProfiles: Profile
    {
        public ServiceRequestMappingProfiles()
        {
            CreateMap<ServiceRequestPostPutDto, ServiceRequest>();
            CreateMap<ServiceRequest, ServiceRequestGetDto>();
        }
    }
}
