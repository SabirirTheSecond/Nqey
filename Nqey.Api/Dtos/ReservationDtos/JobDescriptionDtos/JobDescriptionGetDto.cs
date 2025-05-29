using Nqey.Api.Dtos.ImageDto;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos
{
    public class JobDescriptionGetDto
    {

        public int JobDescriptionId { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public List<ImageGetDto>? Images { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }

    }
}
