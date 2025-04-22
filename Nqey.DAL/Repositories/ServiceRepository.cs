using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;

namespace Nqey.DAL.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly DataContext _dataContext;
        private readonly IUserRepository _userRepo;

        public ServiceRepository(DataContext dataContext, IUserRepository userRepository)
        {
            _dataContext = dataContext;
            _userRepo = userRepository;
        }


        public async Task<List<Service>> GetServicesAsync()
        {
            var services = await _dataContext.Services
                .Include(s=>s.Providers)
                .ToListAsync();
            return services;
            
        }

        public async Task<Service> AddServiceAsync(Service service)
        {
            _dataContext.Services.Add(service);
            await _dataContext.SaveChangesAsync();
            return service;
        }

        public async Task<Service> DeleteServiceAsync(int id)
        {
            var service = await _dataContext.Services
                .FirstOrDefaultAsync(s=>s.ServiceId == id);
            _dataContext.Services.Remove(service);
            await _dataContext.SaveChangesAsync();

            return service;
        }

       public async Task<Service> GetServiceByIdAsync(int id)
        {
            var service = _dataContext.Services.Include(s=>s.Providers)
                .SingleOrDefault(s=>s.ServiceId == id);
            if (service == null)
                return null;
            return service;
            
        }

        public async Task<Service> UpdateServiceAsync(Service updatedService)
        {
            _dataContext.Update(updatedService);
            await _dataContext.SaveChangesAsync();
            return updatedService;
        }
        
        public async Task<List<Provider>> GetAllProviderAsync(int ServiceId)
        {
            var providers = await _dataContext.Providers.Where(p=> p.ServiceId == ServiceId).ToListAsync();
            if(providers == null)
                return null;
            return providers;
        }
        public async Task<Provider> GetProviderByIdAsync(int serviceId, int providerId)
        {
            var provider = await _dataContext.Providers.FirstOrDefaultAsync(p =>  
                p.ServiceId == serviceId &&
                p.ProviderId == providerId
                );
            if (provider == null)
                return null;
            return provider;
        }
        public async Task<Provider> AddProviderAsync(int serviceId,Provider provider)
        {
            var service = await _dataContext.Services
                .Include(s=> s.Providers)
                .FirstOrDefaultAsync(s => s.ServiceId == serviceId);
            if (service == null)
                return null;

            service.Providers.Add(provider);

            await _dataContext.SaveChangesAsync();
            return provider;
        }
        public async Task<Provider> ActivateProviderAsync(int serviceId, Provider provider)
        {
            var service = await _dataContext.Services.FirstOrDefaultAsync(s => s.ServiceId ==serviceId);
            if (service == null)
                return null;

            var user = await _userRepo.GetUserByUserNameAsync(provider.UserName);
            provider.AccountStatus = AccountStatus.Active;
            user.AccountStatus = AccountStatus.Active;
            _dataContext.Providers.Update(provider);
            
            await _dataContext.SaveChangesAsync();
            return provider;
        }

       public async Task<int?> GetProviderIdByUserNameAsync(string userName)
        {
            var providerId = await _dataContext.Providers
                .Where(p => p.UserName == userName)
                .Select(p => (int?)p.ProviderId)
                .FirstOrDefaultAsync();
            
            if (providerId == null)
                return null;

            return providerId;

        }

        public async Task<Provider> UpdateProviderAsync(int serviceId,int providerId, Provider provider)
        {
           var service = _dataContext.Services.FirstOrDefault(s => s.ServiceId== serviceId);
           
            _dataContext.Update(provider);

            await _dataContext.SaveChangesAsync();

            return provider;
        }
    }
}
