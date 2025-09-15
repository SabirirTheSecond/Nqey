using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class ComplaintAttachement
    {
        
            public int Id { get; set; }
            public string FileUrl { get; set; } = default!;
            public int ComplaintId { get; set; }
            //public Complaint Complaint { get; set; } = default!;
        }
    
}
