using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface ISubServiceRepository
    {
        Task<SubService> GetSubServiceByIdAsync(int id);
        Task<SubService> AddSubServiceAsync(SubService subService);
        Task<SubService> DeleteSubServiceAsync(int id);
        Task<List<SubService>> GetSubServicesAsync(int providerId);
    }
}
