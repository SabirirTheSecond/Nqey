using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IServiceRepository
    {
        Task<List<Service>> GetServicesAsync();
        Task<Service> GetServiceByIdAsync(int id);
        Task<Service> AddServiceAsync(Service service);
        Task<Service> UpdateServiceAsync(Service service);
        Task<Service> DeleteServiceAsync(int id);
        Task<List<Provider>> GetAllProviderAsync(int serviceId);
        Task<Provider> GetProviderByIdAsync(int serviceId, int providerId);
        Task<Provider> AddProviderAsync(int serviceId,Provider provider);
    }
}
