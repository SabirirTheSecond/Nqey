using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;
using Nqey.Domain.Helpers;

namespace Nqey.DAL.Repositories
{
    public class ProviderRepository: IProviderRepository
    {
        private readonly DataContext _dataContext;

        public ProviderRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }   

        public async Task<List<Provider>> GetAllProvidersAsync()
        {
            var providers = await _dataContext.Providers
                .Where(p=>p.AccountStatus != AccountStatus.Blocked)
                .Include(p=> p.Reviews)
                .Include(p=>p.ProfileImage)
                .Include(p => p.Portfolio)
                .Include (p => p.Location)
                 .Include(p => p.SentMessages)
                .Include(p => p.ReceivedMessages)
                .ToListAsync();


            return providers;
            
        }

        public async Task<Provider> GetProviderByIdAsync(int userId)
        {
            var provider = await _dataContext.Providers
                .Where(p => p.AccountStatus != AccountStatus.Blocked)
               .Include(p => p.Reviews)
               .Include(p => p.ProfileImage)
               .Include(p => p.Location)
               .Include(p => p.Portfolio)
                .Include(p => p.SentMessages)
                .Include(p => p.ReceivedMessages)
               .FirstOrDefaultAsync(p =>

                       p.UserId == userId
                   );

            if (provider == null)
                return null;
            return provider;
        }

        
    }
}
