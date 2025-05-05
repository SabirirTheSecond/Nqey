using AutoMapper;
using Nqey.Api.Dtos.ProfileImageDtos;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ProfileImageProfiles : Profile
    {
        public ProfileImageProfiles()
        {
            CreateMap<ProfileImagePostPutDto, ProfileImage>();
            CreateMap<ProfileImage, ProfileImageGetDto>();

        }
    }
}
