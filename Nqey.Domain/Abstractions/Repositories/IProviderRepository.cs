using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IProviderRepository
    {
        Task<Provider> GetProviderByIdAsync(int userId);
        Task<List<Provider>> GetAllProvidersAsync();
        Task<List<Provider>> GetProvidersByServicAsync(int serviceId);
      
        Task<int?> GetProviderIdByUserNameAsync(string userName);
        Task<Provider> AddProviderAsync(int? serviceId, Provider provider);
        Task<Provider> ActivateProviderAsync(Provider provider);
        Task<Provider> UpdateProviderAsync(int? serviceId, Provider provider);
        Task<Provider> UpdatePortfolio(int userId,List<PortfolioImage> portfolioImages);
        //Task<Provider> CreateProviderWithUserAndImages();

    }
}
