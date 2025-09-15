using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;

namespace Nqey.DAL.Repositories
{
    public class ServiceRequestRepository(DataContext dataContext): IServiceRequestRepository
    {


       public async Task<ServiceRequest> AddServiceRequestAsync(ServiceRequest serviceRequest, int providerUserId)
        {
            var user = await dataContext.Users.FindAsync(providerUserId);
            if(serviceRequest.ProviderUserId == user?.UserId)
            {

            }
           await dataContext.ServicesRequests.AddAsync(serviceRequest);
            await dataContext.SaveChangesAsync();
            
            return serviceRequest;
        }
        public async Task<ServiceRequest> DeleteServiceRequestAsync(int id)
        {
            var toDelete= await GetServiceRequestByIdAsync(id);
             dataContext.ServicesRequests.Remove(toDelete);
            await dataContext.SaveChangesAsync();
            return toDelete;
            
        }

        public async Task<List<ServiceRequest>> GetAllServiceReqquestsAsync()
        {
            var serviceRequests = await dataContext.ServicesRequests.ToListAsync();
            if (serviceRequests.Count == 0)
            {
                return null;
            }
            return serviceRequests;
            
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(int id)
        {
            var serviceRequest = await dataContext.ServicesRequests.Where(r => r.ServiceRequestId == id)
                .FirstOrDefaultAsync();
            if(serviceRequest == null) 
                return null;

            return serviceRequest;

           
        }

        public Task<ServiceRequest> UpdateServiceRequestAsync(ServiceRequest serviceRequest)
        {
            throw new NotImplementedException();
        }
    }
}
