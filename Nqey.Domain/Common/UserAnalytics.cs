using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Nqey.Domain.Common
{
    [Owned]
    public class UserAnalytics
    {
        public int FiledComplaintsCount { get; set; } = 0;
        public int ComplaintsAgainstCount { get; set; } = 0;
    }
}
