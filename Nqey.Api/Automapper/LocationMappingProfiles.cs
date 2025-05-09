using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class LocationMappingProfiles: Profile
    {
        public LocationMappingProfiles()
        {
            CreateMap<PositionDto, Position>();
            CreateMap<Position, PositionDto>();
            CreateMap<LocationDto, Location>()
                .ForMember(dest => dest.Position, opt => opt.MapFrom(src => src.Position))
                ;
            CreateMap<Location, LocationDto>();

        }
    }
}
