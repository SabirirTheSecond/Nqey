using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos.ReviewDto;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReviewController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReviewService _reviewService;
        private readonly IServiceRepository _serviceRepository;

        public ReviewController(IMapper mapper, IReviewService reviewService
            , IServiceRepository serviceRepository)
        {
            _mapper = mapper;
            _reviewService = reviewService;
            _serviceRepository = serviceRepository;
        }
        [HttpPost]
        public async Task<IActionResult> PostReview([FromBody] ReviewPostPutDto reviewPostPut)
        {

            var domainReview = _mapper.Map<Review>(reviewPostPut);
            var provider = await _serviceRepository.GetProviderByIdAsync(reviewPostPut.ProviderId);
            if (provider == null)
                return NotFound(new ApiResponse<Review>(false, "Could Not Find The Provider"));

            await _reviewService.AddReviewAsync(domainReview);

            var mappedReview = _mapper.Map<ReviewGetDto>(domainReview);

            return Ok(new ApiResponse<ReviewGetDto>(true, "Review posted successfully",mappedReview));

        }
        [HttpGet]
        [Route("providerId")]
        public async Task<IActionResult> GetProviderReviews(int providerId)
        {
            var provider = await _serviceRepository.GetProviderByIdAsync(providerId);
            var providerReviews = await _reviewService.GetAllReviewsByProviderIdAsync(providerId);
            if( provider == null)
                return NotFound(new ApiResponse<Review>(false, "Could Not Find The Provider"));
            var mappedReviews = _mapper.Map<List<ReviewGetDto>>(providerReviews);
            return Ok(new ApiResponse<List<ReviewGetDto>>(true,$"List of {provider.UserName}'s " +
                $"client reviews",mappedReviews));
        }
    }
}
