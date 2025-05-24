using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IReviewService
    {
        public Task<Review> AddReviewAsync(Review review);

        public Task<Review> UpdateReviewAsync(Review review);
        public Task<Review> DeleteReviewAsync(Review review);
        public Task<Review> GetReviewByIdAsync(int reviewId);
        public Task<List<Review>> GetAllReviewsByProviderIdAsync(int providerId);
        public Task<List<Review>> GetAllReviewsAsync();
    }
}
