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
                 .ForMember(dest => dest.ProfilePicture, opt => opt.Ignore());

            CreateMap<Client, ClientPublicGetDto>()
                .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfilePicture));
           
            CreateMap<Client, ClientAdminGetDto>()
                 .ForMember(dest => dest.ProfileImage, opt => opt.MapFrom(src => src.ProfilePicture));

            CreateMap<ClientPostPutDto, UserPostPutDto>();

        }
    }
}
