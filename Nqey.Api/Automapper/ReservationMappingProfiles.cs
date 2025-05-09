using AutoMapper;
using Nqey.Api.Dtos.ReservationDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ReservationMappingProfiles : Profile
    {
        public ReservationMappingProfiles()
        {

            CreateMap<ReservationPostPutDto, Reservation>();
            CreateMap<Reservation, ReservationGetDto>();
            CreateMap<ReservationEvent, ReservationEventDto>()
               //.ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.ReservationEventType.ToString()))
                ;

        }
    }
}
