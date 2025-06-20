﻿using Nqey.Domain.Common;
using Nqey.Domain;
using Nqey.Api.Dtos.ReservationDtos.JobDescriptionDtos;

namespace Nqey.Api.Dtos.ReservationDtos
{
    public class ReservationGetDto
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        //public Client Client { get; set; }
        public int ProviderId { get; set; }
        //public Provider Provider { get; set; }
        public JobDescriptionGetDto JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LocationDto? Location { get; set; }
        public ReservationStatus Status { get; set; }
        public ICollection<ReservationEventDto> Events { get; set; }
        public DateTime createdAt { get; set; }
    }
}
