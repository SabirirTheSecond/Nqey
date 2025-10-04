using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationPatchDto
    {
        //public int ProviderUserId { get; set; }
        public JobDescriptionPostPutDto? JobDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public LocationDto? LocationDto { get; set; }
    }
}
