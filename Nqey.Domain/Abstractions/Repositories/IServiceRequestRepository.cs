using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IServiceRequestRepository
    {
        Task<List<ServiceRequest>> GetAllServiceReqquestsAsync();
        Task<ServiceRequest> GetServiceRequestByIdAsync(int id);
        Task<ServiceRequest> UpdateServiceRequestAsync(ServiceRequest serviceRequest);
        Task<ServiceRequest> DeleteServiceRequestAsync(int id);
        Task<ServiceRequest> AddServiceRequestAsync(ServiceRequest serviceRequest, int providerUserId);
         Task<ServiceRequest> RefuseServiceRequestAsync(int id);
        Task<ServiceRequest> AcceptServiceRequestAsync(int id);

    }
}
