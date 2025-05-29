using AutoMapper;
using Nqey.Api.Dtos.ImageDto;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ImageMappingProfiles: Profile
    {
        public ImageMappingProfiles() 
        {
            CreateMap<ImagePostPutDto, Image>();
            CreateMap<Image,  ImageGetDto>();
        }
    }
}
