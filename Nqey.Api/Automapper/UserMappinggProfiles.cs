using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class UserMappinggProfiles : Profile
    {

        public UserMappinggProfiles() {

            CreateMap<UserPostPutDto, User>();
            CreateMap<User, UserGetDto>();
        }
    }
}
