using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public enum ReservationStatus {Accepted, Pending, Cancelled, Completed};
    public class Reservation
    {

        public int ReservationId { get; set; }
        public int ClientUserId { get; set; }
        public Client Client { get; set; }
        public int ProviderUserId { get; set; }
        public Provider Provider { get; set; }        
        public JobDescription JobDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Location? Location { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime CreatedAt {  get; set; } = DateTime.UtcNow;
        
        public ICollection<ReservationEvent> Events { get; set; } = new List<ReservationEvent>();
        public List<Review> Reviews { get; set; } = new List<Review>();
    
        [NotMapped]
        public bool CanEdit => Status== ReservationStatus.Pending;

    }
}
