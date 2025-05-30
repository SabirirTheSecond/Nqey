using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
       public ReservationController(IMapper mapper, IReservationService reservationService,
           IUserRepository userRepository, IClientRepository clientRepository,
           IImageUploaderService imageUploader) 
        {
            _mapper = mapper;
            _reservationService = reservationService;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _imageUploader = imageUploader;
        }
        [HttpGet]
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
        [Authorize(Policy ="IsOwner")]
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

            domainReservation.ClientId = (int)clientId;
            domainReservation.ProviderId = providerId;
            domainReservation.Status = ReservationStatus.Pending;
            domainReservation.Location = location;
            Console.WriteLine($"domainReservation.ClientId {domainReservation.ClientId.GetType()}");
            
            

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

            await _reservationService.MakeReservationAsync(domainReservation);
            
            
            //await _reservationService.UpdateReservationAsync(domainReservation.ReservationId, domainReservation);
            var mappedReservation = _mapper.Map<ReservationGetDto>(domainReservation);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Added Successfully", mappedReservation));

        }

        [Authorize(Roles = "Client")]
        [HttpPut]
        [Route("{id}/change")]
        public async Task<IActionResult> ChangeReservation(int clientId,int providerId,
            int id, [FromBody] ReservationPostPutDto reservationPostPut)
        {
            
            var existingReservation = await _reservationService.GetReservationByIdAsync(id);
            
            if (existingReservation == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            
            if (existingReservation.Status == ReservationStatus.Accepted)
                return BadRequest(new ApiResponse<Reservation>(false, "Reservation Is Already Accepted"));

            _mapper.Map(reservationPostPut, existingReservation);
            existingReservation.ProviderId = providerId;
            existingReservation.ClientId = clientId;
            
            await _reservationService.UpdateReservationAsync(id, existingReservation);
            var mappedReservation = _mapper.Map<ReservationGetDto>(existingReservation);
            return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Updated Successfully", mappedReservation));
        }

        [Authorize(Roles = "Client")]
        [HttpPut]
        [Route("{id}/cancel")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);
            
            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));
            
            if (existing.Status == ReservationStatus.Accepted)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Is Already Accepted"));
           
            await _reservationService.CancelReservationAsync(id);

            return Ok(new ApiResponse<Reservation>(true, "Reservation Cancelled"));
        }

        [Authorize(Roles ="Provider")]
        [Authorize(Policy ="ActiveAccountOnly")]
        [Authorize(Policy = "IsOwner")]
        [HttpPut]
        [Route("{id}/accept")]
        public async Task<IActionResult> AcceptReservation(int id)
        {
            var existing = await _reservationService.GetReservationByIdAsync(id);

            if (existing == null)
                return NotFound(new ApiResponse<Reservation>(false, "Reservation Not Found"));

            if(existing.Status == ReservationStatus.Pending)
            {
                await _reservationService.AcceptReservationAsync(id);
                var mappedReservation = _mapper.Map<ReservationGetDto>(existing);
                return Ok(new ApiResponse<ReservationGetDto>(true, "Reservation Accepted",mappedReservation));
            }
            return BadRequest(new ApiResponse<Reservation>(false, "Reservation Is Either Accepted Already Or Cancelled"));

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
