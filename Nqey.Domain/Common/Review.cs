using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int ReservationId { get; set; }
        public Reservation Reservation { get; set; }
        public int ClientUserId { get; set; }       
        public int ProviderUserId { get; set; }
        [Range(1,5)]
        public int Stars {  get; set; }
        public string? Feedback { get; set; }
        // Add client        
        public Client Client { get; set; }
        public Provider Provider { get; set; }

        
       

    }
}
