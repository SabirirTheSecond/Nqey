using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IProviderRepository
    {
        Task<Provider> GetProviderByIdAsync(int providerId);
        Task<List<Provider>> GetAllProvidersAsync();

    }
}
