using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.ProfileImageDtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ProviderMapping : Profile
    {
        public ProviderMapping() {
            CreateMap<ProviderPostPutDto, Provider>()
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());
           

            CreateMap<Provider, ProviderPublicGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt
                    .MapFrom(src => src.ProfileImage != null ? 
                         new ProfileImageGetDto
                         {
                             ProfileImageId = src.ProfileImage.ProfileImageId,
                             ImagePath = src.ProfileImage.ImagePath
                         } : null 
                         ));

            CreateMap<Provider, ProviderAdminGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage));

            CreateMap<ProviderPostPutDto, UserPostPutDto>();
        }

    }
}
