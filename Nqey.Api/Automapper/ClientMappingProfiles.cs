using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.ClientDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ClientMappingProfiles : Profile
    {
        public ClientMappingProfiles() 
        {
            CreateMap<ClientPostPutDto, Client>()
                 .ForMember(dest => dest.ProfileImage, opt => opt.Ignore());

            CreateMap<Client, ClientPublicGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage));
           
            CreateMap<Client, ClientAdminGetDto>()
                 .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfileImage));

            CreateMap<ClientPostPutDto, UserPostPutDto>();
            CreateMap<ClientPatchDto, Client>()
                .ForMember(dest => dest.ProfileImage, opt => opt.Ignore())
                .ForAllMembers(opt=> opt.Condition((src, dest, srcMember)=> srcMember!=null))
                ;

        }
    }
}
