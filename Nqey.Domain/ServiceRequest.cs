using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public enum ServiceRequestStatus { Pending, Accepted, Refused};
    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }
        public string RequestName { get; set; }
        public string Description { get; set; }
        public int? ProviderUserId { get; set; }
        public Provider? Provider { get; set; }

        public ServiceRequestStatus ServiceRequestStatus { get; set; } = ServiceRequestStatus.Pending;

    }
}
