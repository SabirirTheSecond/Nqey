using AutoMapper;
using Nqey.Api.Dtos.ImageDto;
using Nqey.Api.Dtos.ReservationDtos;
using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Automapper
{
    public class ReservationMappingProfiles : Profile
    {
        public ReservationMappingProfiles()
        {

            CreateMap<ReservationPostPutDto, Reservation>()
                .ForMember(dest=> dest.JobDescription, opt=> opt.Ignore())
                .ForMember(dest => dest.Location, opt => opt.Ignore())
                .ForPath(dest => dest.JobDescription.Images, opt=> opt.Ignore())
                ;
            CreateMap<Reservation, ReservationGetDto>();
            CreateMap<ReservationEvent, ReservationEventDto>()
                ;
            CreateMap<JobDescriptionPostPutDto, JobDescription>()
                .ForMember(dest => dest.Images, opt=> opt.Ignore())
                ;
            
            CreateMap<JobDescription, JobDescriptionGetDto>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images))
                ;
            CreateMap<ImagePostPutDto, Image>();
            CreateMap<Image, ImageGetDto>();


        }
    }
}
