﻿using Nqey.Domain.Common;
using Nqey.Domain;

namespace Nqey.Api.Dtos
{
    public class ReservationGetDto
    {
        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        //public Client Client { get; set; }
        public int ProviderId { get; set; }
        //public Provider Provider { get; set; }
        public string JobDescription { get; set; }
        public Location? Location { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime createdAt { get; set; }
    }
}
