using Nqey.Domain.Common;
using Nqey.Domain;
using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Api.Dtos.ReviewDto;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationGetDto
    {
        public int ReservationId { get; set; }
        public int ClientUserId { get; set; }
        //public Client Client { get; set; }
        public int ProviderUserId { get; set; }
        public ProviderPublicGetDto Provider { get; set; }
        public JobDescriptionGetDto JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDto? Location { get; set; }
        public ReservationStatus Status { get; set; }
        public ICollection<ReservationEventDto> Events { get; set; }
        public List<ReviewGetDto> Reviews { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
