using AutoMapper;
using Nqey.Api.Dtos.ServiceDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ServiceMappingProfiles : Profile
    {
        public ServiceMappingProfiles() {

            CreateMap<ServicePostPutDto, Service>()
                .ForMember(dest => dest.ServiceImage, opt => opt.Ignore())
                ;

            CreateMap<Service, ServiceAdminGetDto>()
     .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ServiceImage)); // Map the entire Image object

            CreateMap<Service, ServicePublicGetDto>()
        .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ServiceImage)); // Map the entire Image object


        }
    }
}
