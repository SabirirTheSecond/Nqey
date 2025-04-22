using Nqey.Domain;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationPostPutDto
    {
        public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public string JobDescription { get; set; }

    }
}
