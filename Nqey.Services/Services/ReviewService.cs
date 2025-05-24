using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<Review> AddReviewAsync(Review review, int providerId)
        {
            var provider = await _serviceRepo.GetProviderByIdAsync(providerId);
            throw new NotImplementedException();
        }

        public Task<Review> DeleteReviewAsync(Review review)
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetAllReviewsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Review> GetReviewByIdAsync(int reviewId)
        {
            throw new NotImplementedException();
        }

        public Task<Review> UpdateReviewAsync(Review review)
        {
            throw new NotImplementedException();
        }
    }
}
