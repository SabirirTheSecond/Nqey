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
    public class ProviderRepository: IProviderRepository
    {
        private readonly DataContext _dataContext;

        public async Task<List<Provider>> GetAllProvidersAsync()
        {
            var providers = await _dataContext.Providers
                .Include(p=>p.ProfileImage)
                .Include(p => p.Portfolio)
                .Include (p => p.Location)
                //.Include(p => p.)
                .ToListAsync();
            if (!providers.Any())
                throw new NullReferenceException();
            return providers;
            
        }

        public Task<Provider> GetProviderByIdAsync(int providerId)
        {
            throw new NotImplementedException();
        }
    }
}
