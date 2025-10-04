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
                //.Where(p=>p.AccountStatus != AccountStatus.Blocked)
                .Include(p=> p.Reviews)
                .Include(p=>p.ProfileImage)
                .Include(p => p.Portfolio)
                .Include (p => p.Location)
                .Include(p => p.IdentityPiece)
               .Include(p => p.SelfieImage)
                 .Include(p => p.SentMessages)
                .Include(p => p.ReceivedMessages)
                .Include(p=>p.FiledComplaints)
                .Include(p=>p.ComplaintsAgainst)
                .ToListAsync();


            return providers;
            
        }
        public async Task<List<Provider>> GetProvidersByServicAsync(int ServiceId)
        {
            var providers = await _dataContext.Providers
                .Where(p => p.ServiceId == ServiceId)
                .Include(p => p.Reviews)
                .Include(p => p.ProfileImage)
                .Include(p => p.Location)
                .Include(p => p.Portfolio)
                .Include(p => p.FiledComplaints)
                .Include(p => p.ComplaintsAgainst)
                .ToListAsync();
            if (providers == null)
                return null;
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
               .Include(p=>p.IdentityPiece)
               .Include(p=>p.SelfieImage)
                .Include(p => p.SentMessages)
                .Include(p => p.ReceivedMessages)
                .Include(p => p.FiledComplaints)
                .Include(p => p.ComplaintsAgainst)
               .FirstOrDefaultAsync(p =>

                       p.UserId == userId
                   );
            Console.WriteLine(provider.AverageRating);

            if (provider == null)
                return null;
            return provider;
        }
        public async Task<Provider> AddProviderAsync(int? serviceId, Provider provider)
        {
            var service = await _dataContext.Services
                .Include(s => s.Providers)
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (service == null)
                return null;

            service.Providers.Add(provider);

            await _dataContext.SaveChangesAsync();
            // Ensure EF re-hydrates the keys (important with TPT inheritance)
            await _dataContext.Entry(provider).ReloadAsync();
            return provider;
        }
        public async Task<Provider> ActivateProviderAsync(Provider provider)
        {
            var service = await _dataContext.Services.FirstOrDefaultAsync(s => s.ServiceId == provider.ServiceId);
            if (service == null)
                return null;


            provider.AccountStatus = AccountStatus.Active;

            _dataContext.Providers.Update(provider);

            await _dataContext.SaveChangesAsync();
            return provider;
        }

        public async Task<int?> GetProviderIdByUserNameAsync(string userName)
        {
            var providerId = await _dataContext.Providers
                .Where(p => p.UserName == userName)
                .Select(p => (int?)p.UserId)
                .FirstOrDefaultAsync();

            if (providerId == null)
                return null;

            return providerId;

        }

        public async Task<Provider> UpdateProviderAsync(int? serviceId, Provider provider)
        {
            var service = _dataContext.Services.FirstOrDefault(s => s.ServiceId == serviceId);

            _dataContext.Update(provider);

            await _dataContext.SaveChangesAsync();

            return provider;
        }
        public async Task<Provider> UpdatePortfolio(int userId,List<PortfolioImage> portfolioImages)
        {
            var provider = await GetProviderByIdAsync(userId);
            if (provider != null)
            {
                //provider.Portfolio = portfolioImages;
                provider.Portfolio ??= new List<PortfolioImage>();
                provider.Portfolio.AddRange(portfolioImages);

                await _dataContext.SaveChangesAsync();
                return provider;
            }
            return null;
            
        }

    }
}
