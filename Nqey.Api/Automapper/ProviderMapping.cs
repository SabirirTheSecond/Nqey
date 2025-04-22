using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ProviderMapping : Profile
    {
        public ProviderMapping() {
            CreateMap<ProviderPostPutDto, Provider>()
            .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());
           

            CreateMap<Provider, ProviderPublicGetDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<Provider, ProviderAdminGetDto>()
                .ForMember(dest => dest.ProfilePicture, opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<ProviderPostPutDto, UserPostPutDto>();
        }

    }
}
