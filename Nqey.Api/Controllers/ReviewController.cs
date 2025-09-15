using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos.ReviewDto;
using Nqey.DAL;
using Nqey.Domain;
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
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IReservationService _reservationService;
        private readonly DataContext _dataContext;

        public ReviewController(IMapper mapper, IReviewService reviewService
            , IServiceRepository serviceRepository, IUserRepository userRepository,
            IClientRepository clientRepository, IReservationService reservationService, DataContext dataContext)
        {
            _mapper = mapper;
            _reviewService = reviewService;
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _reservationService = reservationService;
            _dataContext = dataContext;
        }
        [Authorize(Roles ="Client")]
        [HttpPost]

        public async Task<IActionResult> PostReview([FromBody] ReviewPostPutDto reviewPostPut)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            Console.WriteLine($"userIdClaim = {userIdClaim}");
 
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized(new ApiResponse<Review>(false, "Invalid user ID"));
            }
            var reservation = await _reservationService.GetReservationByIdAsync(reviewPostPut.ReservationId);
            var provider = await _serviceRepository.GetProviderByIdAsync(reservation.ProviderUserId);
            if (provider == null)
                return NotFound(new ApiResponse<Review>(false, "Could Not Find The Provider"));

            var user = await _userRepository.GetByIdAsync(userId);

            var clientId = userId;
            if (clientId == 0)
                return NotFound(new ApiResponse<Reservation>(false, "Cannot determine client id"));

            var hasReservation = await _dataContext.Reservations
                .AnyAsync(r => r.ProviderUserId == provider.UserId
                    && r.ClientUserId == clientId
                    && (r.Status == ReservationStatus.Accepted 
                            || r.Status == ReservationStatus.Completed));
            var hasReview = await _reviewService.AlreadyReviewed(reviewPostPut.ReservationId);

            if (hasReview)
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse<Review>(false, "You can post only one review per reservation"));
            }
            if(!hasReservation)
             {
               return BadRequest(new ApiResponse<Reservation>(false, "You can't review a provider without" +
                            "a confirmed reservation !"));
                    }

            var domainReview = _mapper.Map<Review>(reviewPostPut);
            domainReview.ClientUserId =(int)clientId;
            domainReview.ProviderUserId = provider.UserId;

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
