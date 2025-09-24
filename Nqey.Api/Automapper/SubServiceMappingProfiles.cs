using AutoMapper;
using Nqey.Api.Dtos.SubServiceDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class SubServiceMappingProfiles: Profile
    {
        public SubServiceMappingProfiles() {
            CreateMap<SubServicePostDto, SubService>();
            CreateMap<SubServicePatchDto, SubService>();
            CreateMap<SubService, SubServiceGetDto>();

        }
    }
}
