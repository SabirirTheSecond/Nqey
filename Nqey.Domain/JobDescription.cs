using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public class JobDescription
    {
        public int JobDescriptionId { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; }
        public List<Image>? Images { get; set; }
        public int ReservationId {  get; set; }
        public Reservation Reservation { get; set; }
    }
}
