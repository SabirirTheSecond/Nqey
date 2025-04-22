using AutoMapper;
using Nqey.Api.Dtos.ServiceDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ServiceMappingProfiles : Profile
    {
        public ServiceMappingProfiles() {
            CreateMap<Service, ServicePublicGetDto>();
            CreateMap<ServicePostPutDto, Service>();
           
        }
    }
}
