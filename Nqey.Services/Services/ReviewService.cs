using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Services.Services
{
    public class ReviewService : IReviewService
    {
        private readonly DataContext _dataContext;
        private readonly IServiceRepository _serviceRepo;
        private readonly IClientRepository _clientRepo;


        public ReviewService(DataContext dataContext, IServiceRepository serviceRepository,
            IClientRepository clientRepository) 
        {
           _dataContext = dataContext;
            _serviceRepo = serviceRepository;
            _clientRepo = clientRepository;

        }

        public async Task<Review> AddReviewAsync(Review review)
        {
            var provider = await _serviceRepo.GetProviderByIdAsync(review.ProviderId);
           
            if (provider == null)
                return null;

            await _dataContext.Reviews.AddAsync(review);
            await _dataContext.SaveChangesAsync();

          return review ;
            
        }
       

        public Task<Review> DeleteReviewAsync(Review review)
        {
            throw new NotImplementedException();
        }

        

        public async Task<List<Review>> GetAllReviewsByProviderIdAsync(int providerId)
        {
            var reviews = await _dataContext.Reviews
                .Where(r => r.ProviderId == providerId)
                .Include(r=> r.Client)
                    .ThenInclude(c=> c.ProfileImage)
                
                .ToListAsync();
            if (reviews == null)
                throw new NullReferenceException();

                
            return reviews;
        }

        public Task<Review> GetReviewByIdAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<Review> UpdateReviewAsync(Review review)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Review>> GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }
    }
}
