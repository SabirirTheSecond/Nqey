using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos.ReservationDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _reservationService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IImageUploaderService _imageUploader;
        private readonly IProviderRepository _providerRepository;
       public ReservationController(IMapper mapper, IReservationService reservationService,
           IUserRepository userRepository, IClientRepository clientRepository,
           IImageUploaderService imageUploader, IProviderRepository providerRepo) 
        {
            _mapper = mapper;
            _reservationService = reservationService;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _imageUploader = imageUploader;
            _providerRepository = providerRepo;
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        
        public async Task<ActionResult> GetReservations()
        {
            
                var reservations = await _reservationService.GetReservationsAsync();
                if (reservations == null)
                    return NotFound(new ApiResponse<Reservation>(false, "Reservations Not Found"));
                var mappedReservations = _mapper.Map<List<ReservationGetDto>>(reservations);
                return Ok(new ApiResponse<List<ReservationGetDto>>(true, "Reservations Retrieved Succussfully"
                    , mappedReservations));     
        }

        [Authorize(Roles ="Admin,Provider,Client")]
        [Authorize(Policy ="ActiveAccountOnly")]
        [Authorize(Policy = "IsReservationOwner")]
        [HttpGet]

        [Route("{id}")]
        public async Task<ActionResult> GetReservationById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null)
                return NotFound(new ApiResponse<Reservation>(false, $"Reservation With Id ={id} Not Found"));
            var mappedReservation = _mapper.Map<ReservationGetDto>(reservation);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation retrieved Successfully", mappedReservation));
        }
        //[HttpGet("my_reservations")]
        //[Authorize] // no custom policy, just authenticated
        //public async Task<IActionResult> GetMyReservations()
        //{
        //    var userId = int.Parse(User.FindFirst("userId")!.Value);

        //    var reservations = await _context.Reservations
        //        .Where(r => r.Client.UserId == userId || r.Provider.UserId == userId)
        //        .ToListAsync();

        //    return Ok(reservations);
        //}
        [Authorize(Roles = "Admin,Client")]
        [Authorize(Policy = "ActiveAccountOnly")]
        //[Authorize(Policy = "IsReservationOwner")]
        [HttpGet]
        [Route("client_reservations")]
        public async Task<ActionResult> GetReservationByClientId()
        {
            var userIdClaim = User.FindFirstValue("userId");
            Console.WriteLine($" userIdClaim value after declaration : {userIdClaim}");
            if (userIdClaim == null)
            {
                Console.WriteLine($" userIdClaim error : {userIdClaim}");
                return BadRequest(new ApiResponse<Reservation>(false, "Issue With Your Authentication"));
            }

            var userId = int.Parse(userIdClaim);
            Console.WriteLine($"userId: {userId}");
            var user = await _userRepository.GetByIdAsync(userId);
            var clientId = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
            Console.WriteLine($"clientId: {clientId.GetType()}");
            
            if (clientId == null)
                return BadRequest(new ApiResponse<Reservation>(false, "Couldn't determine Client Identity"));
           
            var reservations = await _reservationService.GetReservationByClientIdAsync((int)clientId);
            
            if (reservations == null)
                return NotFound(new ApiResponse<Reservation>(false, $"Client With Id n°={clientId} has no reservations"));


            var mappedReservations = _mapper.Map<List<ReservationGetDto>>(reservations);
            return Ok(new ApiResponse<List<ReservationGetDto>>(true,
                "Reservations retrieved Successfully", mappedReservations));
        }



        [Authorize(Roles = "Admin,Provider")]
        [Authorize(Policy = "ActiveAccountOnly")]
        //[Authorize(Policy = "IsReservationOwner")]
        [HttpGet]

        [Route("provider_reservations")]
        public async Task<ActionResult> GetReservationByProvider()
        {
            var userIdClaim = User.FindFirstValue("userId");
            Console.WriteLine($" userIdClaim value after declaration : {userIdClaim}");
            if (userIdClaim == null)
            {
                Console.WriteLine($" userIdClaim error : {userIdClaim}");
                return BadRequest(new ApiResponse<Reservation>(false, "Issue With Your Authentication"));
            }

            if(!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Object>(false, "Not Authorized"));
            }

            var reservations = await _reservationService.GetReservationByProviderIdAsync(userId);
            if (reservations == null)
                return NotFound(new ApiResponse<Reservation>(false, $"Provider With Id n°={userId} has no reservations"));
            var mappedReservations = _mapper.Map<List<ReservationGetDto>>(reservations);
            return Ok(new ApiResponse<List<ReservationGetDto>>(true,
                "Reservations retrieved Successfully", mappedReservations));
        }

        [Authorize(Roles = "Client")]
        [HttpPost]
        public async Task<IActionResult> MakeReservation( int providerId,
           [FromForm] ReservationPostPutDto reservationPostPut)
        {
            
            var domainReservation = _mapper.Map<Reservation>(reservationPostPut);
            var location = _mapper.Map<Location>(reservationPostPut.LocationDto);
            var jobDescription = _mapper.Map<JobDescription>(reservationPostPut.JobDescription);
            var userIdClaim = User.FindFirst("userId");
            Console.WriteLine($" userIdClaim after declaration : {userIdClaim}");
            if (userIdClaim == null)
                Console.WriteLine($" userIdClaim error : {userIdClaim}");
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.ToDictionary(

            //        key => key.Key,
            //        value => value.Value.Errors.Select(e=> e.ErrorMessage).ToArray()
            //        );
            //    //return BadRequest(new ApiResponse<string>(false, "Validation Errors",errors));

            //}
             string? imagePath = null;
            //var images= reservationPostPut.JobDescription.Images ;
           
            var userId = int.Parse(userIdClaim.Value);
            Console.WriteLine($"userId: {userId}");
            var user = await _userRepository.GetByIdAsync(userId);
            var clientId = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
            Console.WriteLine($"clientId: {clientId.GetType()}");
            if (clientId == null)
                return BadRequest(new ApiResponse<Reservation>(false, "Cannot determine client id"));

            domainReservation.ClientUserId = (int)clientId;
            domainReservation.ProviderUserId = providerId;
            domainReservation.Status = ReservationStatus.Pending;
            domainReservation.Location = location;
            Console.WriteLine($"domainReservation.ClientId {domainReservation.ClientUserId.GetType()}");
                        
            // 4. Ensure JobDescription exists
            //var jobDescription = new JobDescription();
            var images = reservationPostPut.JobDescription?.Images;

            if (images != null && images.Any())
            {
                jobDescription.Images = new List<Image>();
                foreach (var file in reservationPostPut.JobDescription.Images)

                {
                    try
                    {
                        imagePath = await _imageUploader.UploadImageToSupabase((IFormFile)file);
                        jobDescription.Images.Add(new Image
                        {
                            ImagePath = imagePath
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new ApiResponse<string>(false, "Failed to upload job images to Supabase", ex.Message));

                    }
                }

            }
            var client = await _clientRepository.GetClientByIdAsync(userId);
            domainReservation.JobDescription = jobDescription;
            // Booking timeline tracker
            domainReservation.Events.Add(
                new ReservationEvent
                {
                    ReservationEventType = ReservationEventType.Pending,
                    CreatedAt = DateTime.UtcNow,
                    Notes = "Reservation Added"
                }
                );
            client.ClientAnalytics.Bookings++;
            await _reservationService.MakeReservationAsync(domainReservation);
            
            
            //await _reservationService.UpdateReservationAsync(domainReservation.ReservationId, domainReservation);
            var mappedReservation = _mapper.Map<ReservationGetDto>(domainReservation);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Added Successfully", mappedReservation));

        }

        [Authorize(Roles = "Client")]
        [HttpPut]
        [Route("{id}/change")]
        public async Task<IActionResult> ChangeReservation(
            int id, [FromForm] ReservationPostPutDto reservationPostPut)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Reservation>(false, "Authentication Error, Try To Login Again"));
            }
            var user = await _userRepository.GetByIdAsync(userId);
            
            var existingReservation = await _reservationService.GetReservationByIdAsync(id);
            
            if (existingReservation == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            
            if (existingReservation.Status == ReservationStatus.Accepted)
                return BadRequest(new ApiResponse<Reservation>(false, "Reservation Is Already Accepted"));

            var location = _mapper.Map<Location>(reservationPostPut.LocationDto);
            var jobDescription = _mapper.Map<JobDescription>(reservationPostPut.JobDescription);
            
            string? imagePath = null;
            existingReservation.Location = location;
            var images = reservationPostPut.JobDescription?.Images;

            if (images != null && images.Any())
            {
                jobDescription.Images = new List<Image>();
                foreach (var file in reservationPostPut.JobDescription.Images)

                {
                    try
                    {
                        imagePath = await _imageUploader.UploadImageToSupabase((IFormFile)file);
                        jobDescription.Images.Add(new Image
                        {
                            ImagePath = imagePath
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new ApiResponse<string>(false, "Failed to upload job images to Supabase", ex.Message));

                    }
                }

            }
            existingReservation.JobDescription = jobDescription;

            var updatedReservation = await _reservationService.UpdateReservationAsync(id, existingReservation);
            var mappedReservation = _mapper.Map<ReservationGetDto>(updatedReservation);

            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Updated Successfully", mappedReservation));
        }

        [Authorize(Roles = "Client")]
        [Authorize(Policy = "IsReservationOwner")]
        [HttpPut]
        [Route("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Reservation>(false, "Could Not Determine User Identity," +
                    "Please Log In Again Or Contact Support"));
            }
            var client = await _clientRepository.GetClientByIdAsync(userId);
            var existing = await _reservationService.GetReservationByIdAsync(id);
            
            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            
            if (existing.Status == ReservationStatus.Completed || existing.Status == ReservationStatus.Cancelled)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Is Already Completed" +
                    " Or Cancelled"));
            client.ClientAnalytics.Cancelations++;
            await _reservationService.CancelReservationAsync(id);
            var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Cancelled",mappedReservation));
        }

        [Authorize(Roles ="Provider")]
        [Authorize(Policy ="ActiveAccountOnly")]
        [Authorize(Policy = "IsReservationOwner")]
        [HttpPut]
        [Route("{id}/accept")]
        public async Task<IActionResult> AcceptReservation(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);
            var userIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Reservation>(false, "Could Not Determine User Identity," +
                    "Please Log In Again Or Contact Support"));
            }
            var provider = await _providerRepository.GetProviderByIdAsync(userId);
            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));

            if(existing.Status == ReservationStatus.Pending)
            {
                await _reservationService.AcceptReservationAsync(id);
                var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
                provider.ProviderAnalytics.Accepts += 1;
                return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Accepted",mappedReservation));
            }
            return BadRequest(new ApiResponse<Reservation>(false, "Reservation Is Either Accepted Already Or Cancelled"));

        }
        [Authorize(Roles = "Provider")]
        [Authorize(Policy = "ActiveAccountOnly")]
        [Authorize(Policy = "IsReservationOwner")]
        [HttpPut]
        [Route("{id}/refuse")]
        public async Task<IActionResult> RefuseReservation(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Reservation>(false, "Could Not Determine User Identity," +
                    "Please Log In Again Or Contact Support"));
            }
            var provider = await _providerRepository.GetProviderByIdAsync(userId);
            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));

            if (existing.Status == ReservationStatus.Pending)
            {
                await _reservationService.RefuseReservationAsync(id);
                var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
                provider.ProviderAnalytics.Refuses += 1;
                return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Rejected", mappedReservation));
            }
            return BadRequest(new ApiResponse<Reservation>(false, "Reservation Is Either Accepted Already Or Cancelled"));

        }
        [Authorize(Roles = "Provider")]
        [Authorize(Policy = "ActiveAccountOnly")]
        [Authorize(Policy = "IsReservationOwner")]
        [HttpPut]
        [Route("{id}/completed")]
        public async Task<IActionResult> CompletedtReservation(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);

            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            var provider = await _providerRepository.GetProviderByIdAsync(existing.ProviderUserId);
            if (existing.Status == ReservationStatus.Accepted)
            {
                await _reservationService.CompletedReservationAsync(id);
                var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
                provider.ProviderAnalytics.JobsDone += 1;
                provider.ProviderAnalytics.Completions += 1;
                provider.JobsDone += 1;
                return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Completed", mappedReservation));
            }
            return BadRequest(new ApiResponse<Reservation>(false, "You Can Only Mark An Accepted Reservation As Completed"));

        }

        [Authorize(Roles = "Client")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteReservations(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);
            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            await _reservationService.DeleteReservationAsync(id);
            var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Deleted", mappedReservation));
        }

    }
}
