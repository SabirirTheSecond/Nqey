using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationPostPutDto
    {
        //public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public JobDescriptionPostPutDto JobDescription { get; set; }
        public LocationDto? LocationDto { get; set; }

    }
}
