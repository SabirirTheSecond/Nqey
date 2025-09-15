using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public enum ReservationEventType
    {
        Pending,
        Accepted,
        Changed,
        InProgres,
        Completed,
        Cancelled,
        Rejected
    }
    public class ReservationEvent
    {
        
            public int ReservationEventId { get; set; }

            public int ReservationId { get; set; }
            public Reservation Reservation { get; set; }

            public ReservationEventType ReservationEventType { get; set; }

            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

            public string? Notes { get; set; }
        
    }
}
