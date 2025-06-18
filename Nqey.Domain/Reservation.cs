using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public enum ReservationStatus {Accepted, Pending, Cancelled};
    public class Reservation
    {

        public int ReservationId { get; set; }
        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
        
        public JobDescription JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Location? Location { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime createdAt {  get; set; } = DateTime.UtcNow;
        
        public ICollection<ReservationEvent> Events { get; set; } = new List<ReservationEvent>();

    }
}
