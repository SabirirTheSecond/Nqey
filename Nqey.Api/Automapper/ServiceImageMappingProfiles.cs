using AutoMapper;
using Nqey.Api.Dtos.ServiceDtos;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ServiceImageMappingProfiles: Profile
    {
        public ServiceImageMappingProfiles()
        {
            CreateMap<ServiceImagePostPut, Image>();
            CreateMap<Image, ServiceImageGetDto>();
        }

    }
}
