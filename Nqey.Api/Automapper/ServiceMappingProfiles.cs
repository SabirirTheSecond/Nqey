using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ServiceMappingProfiles : Profile
    {
        public ServiceMappingProfiles() {
            CreateMap<Service, ServiceGetDto>();
            CreateMap<ServicePostPutDto, Service>();
           
        }
    }
}
