using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nqey.Domain.Common
{
    [Owned]
    public class ClientAnalytics : UserAnalytics
    {
        public int Bookings { get; set; }      
        public int Cancelations { get; set; }
        //public int FiledComplaintsCount { get; set; }
        //public int ComplaintsAgainstCount { get; set; }
    }
}
