using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nqey.Domain.Common
{
    [Owned]
    public class ProviderAnalytics : UserAnalytics
    {
        public int JobsDone { get; set; } 
        public int Accepts { get; set; }
        public int Refuses { get; set; }
        public int Completions { get; set; }
        //public int CompletionRate { get; set; } 
        //public int CompletedThisMonth { get; set; }
        //public int CancellationRate { get; set; }
        //public int ServicesOffered { get; set; } 
        //public int PendingOrders { get; set; }
        //public DateTime NextAppointement {  get; set; }


        //public int FiledComplaintsCount { get; set; }
        //public int ComplaintsAgainstCount { get; set; }

    }
}
