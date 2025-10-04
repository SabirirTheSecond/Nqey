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
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
            .ForMember(dest => dest.Portfolio, opt => opt.Ignore())
            .ForMember(dest => dest.IdentityPiece, opt => opt.Ignore())
            .ForMember(dest => dest.SelfieImage, opt => opt.Ignore());

            CreateMap<ProviderPatchDto, Provider>()
            .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
            .ForMember(dest => dest.Portfolio, opt => opt.Ignore())
            .ForMember(dest => dest.IdentityPiece, opt => opt.Ignore())
            .ForMember(dest => dest.SelfieImage, opt => opt.Ignore())
            .ForAllMembers(opt=>opt.Condition((src,dest,srcMember)=>srcMember !=null))
            ;
            CreateMap<ProviderAdminPatchDto, Provider>()
           .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
           .ForMember(dest => dest.Portfolio, opt => opt.Ignore())
           .ForMember(dest => dest.IdentityPiece, opt => opt.Ignore())
           .ForMember(dest => dest.SelfieImage, opt => opt.Ignore())
           .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null))
           ;
            CreateMap<Provider, ProviderPublicGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt
                    .MapFrom(src => src.ProfileImage != null ? 
                         new ProfileImageGetDto
                         {
                             ProfileImageId = src.ProfileImage.ProfileImageId,
                             ImagePath = src.ProfileImage.ImagePath
                         } : null 
                         ))
                .ForMember(dest => dest.Portfolio, opt => opt.MapFrom(src => src.Portfolio))
                //.ForMember(dest => dest.IdentityPiece, opt => opt.MapFrom(src => src.IdentityPiece))
                //.ForMember(dest => dest.SelfieImage, opt => opt.MapFrom(src => src.SelfieImage)); 
            ;
                
            CreateMap<Provider, ProviderAdminGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage))
                .ForMember(dest => dest.IdentityPiece, opt => opt.MapFrom(src => src.IdentityPiece))
            .ForMember(dest => dest.SelfieImage, opt => opt.MapFrom(src => src.SelfieImage));
            CreateMap<ProviderPostPutDto, UserPostPutDto>();
        }

    }
}
