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
    public class SubServiceRepository(DataContext dataContext,
        IProviderRepository providerRepository) : ISubServiceRepository
    {
        public async Task<SubService> AddSubServiceAsync(SubService subService)
        {
            await dataContext.SubServices.AddAsync(subService);
            await dataContext.SaveChangesAsync();
            return subService;
        }

        public async Task<SubService> DeleteSubServiceAsync(int id)
        {
            var toDelete = await dataContext.SubServices
                .Include(s => s.Provider)
                .FirstOrDefaultAsync(s=>s.SubServiceId== id);
            if (toDelete == null)
                return null;
            dataContext.SubServices.Remove(toDelete);
            await dataContext.SaveChangesAsync();
            return toDelete;
        }

        public async Task<SubService> GetSubServiceByIdAsync(int id)
        {
            var subService = await dataContext.SubServices
                .Include(s => s.Provider)
                .FirstOrDefaultAsync(s => s.SubServiceId == id);
            if (subService == null)
                return null;
            return subService;
        }

        public async Task<List<SubService>> GetSubServicesAsync(int providerId)
        {
            var provider = await providerRepository.GetProviderByIdAsync(providerId);
            if (provider == null)
                return null;

            var subServices = await dataContext.SubServices
                .Where(s=>s.ProviderUserId== providerId)
                .Include(s=> s.Provider)
                .ToListAsync();
            return subServices;
            
        }
    }
}
