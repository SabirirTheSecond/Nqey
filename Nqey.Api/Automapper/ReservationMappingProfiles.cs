using AutoMapper;
using Nqey.Api.Dtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ReservationMappingProfiles : Profile
    {
        public ReservationMappingProfiles()
        {

            CreateMap<ReservationPostPutDto, Reservation>();
            CreateMap<Reservation, ReservationGetDto>();
        }
    }
}
