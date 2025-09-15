using AutoMapper;
using Nqey.Api.Dtos.ComplaintDtos;
using Nqey.Domain;

namespace Nqey.Api.Automapper
{
    public class ComplaintMappingProfiles: Profile
    {
        public ComplaintMappingProfiles() {

            CreateMap<ComplaintPostPutDto, Complaint>()
                .ForMember(dest=>dest.Attachments, opt=>opt.Ignore())
                ;
            CreateMap<Complaint, ComplaintGetDto>()
                .ForMember(dest => dest.Attachments, opt => opt.MapFrom(src => src.Attachments)) ;

        }
    }
}
