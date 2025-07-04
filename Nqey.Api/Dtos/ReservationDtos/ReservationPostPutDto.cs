﻿using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationPostPutDto
    {
        //public int ClientId { get; set; }
        public int ProviderId { get; set; }
        public JobDescriptionPostPutDto JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDto? LocationDto { get; set; }

    }
}
