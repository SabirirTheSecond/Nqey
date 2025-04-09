using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ClientMappingProfiles : Profile
    {
        public ClientMappingProfiles() 
        {
            CreateMap<ClientPostPutDto, Client>();
            CreateMap<Client, ClientGetDto>();
            CreateMap<ClientPostPutDto, UserPostPutDto>();

        }
    }
}
