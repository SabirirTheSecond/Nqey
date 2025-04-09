using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ProviderMapping : Profile
    {
        public ProviderMapping() {
            CreateMap<ProviderPostPutDto, Provider>();
            CreateMap<Provider, ProviderGetDto>();
            CreateMap<ProviderPostPutDto, UserPostPutDto>();
        }

    }
}
