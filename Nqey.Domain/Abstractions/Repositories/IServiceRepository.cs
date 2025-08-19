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
        Task<List<Provider>> GetProvidersByServicAsync(int serviceId);
        Task<List<Provider>> GetAllProvidersAsync();
        Task<Provider> GetProviderByIdAsync( int providerId);
        Task<int?> GetProviderIdByUserNameAsync(string userName);
        Task<Provider> AddProviderAsync(int serviceId,Provider provider);
        Task<Provider> ActivateProviderAsync(Provider provider);
        Task<Provider> UpdateProviderAsync(int serviceId, int providerId, Provider provider);
        Task<List<Provider>> GetPreRegisteredProviders();
        Task<Provider> GetPreRegisteredProviderById(int providerId);
    }
}
