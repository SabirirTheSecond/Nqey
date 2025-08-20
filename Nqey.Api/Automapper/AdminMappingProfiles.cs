using AutoMapper;
using Nqey.Api.Dtos.AdminDtos;
using Nqey.Api.Dtos.ClientDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class AdminMappingProfiles : Profile
    {

        public AdminMappingProfiles() {

            CreateMap<AdminPostPutDto, Admin>();
            CreateMap<Admin, AdminGetDto>();

        }
    }
}
