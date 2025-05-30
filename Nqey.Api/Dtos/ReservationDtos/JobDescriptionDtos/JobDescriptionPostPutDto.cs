using Nqey.Api.Dtos.ImageDto;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos
{
    public class JobDescriptionPostPutDto
    {

        public string? Title { get; set; }
        public string Description { get; set; }
        public List<IFormFile>? Images { get; set; }
        //public int ReservationId { get; set; }

    }
}
