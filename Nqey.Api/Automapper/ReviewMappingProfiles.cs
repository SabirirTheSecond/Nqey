using AutoMapper;
using Nqey.Api.Dtos.ReviewDto;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ReviewMappingProfiles: Profile
    {
        public ReviewMappingProfiles() 
        {
            CreateMap<ReviewPostPutDto, Review>();
            CreateMap<Review, ReviewGetDto>();
        }

    }
}
