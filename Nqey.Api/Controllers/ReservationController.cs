using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
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

       public ReservationController(IMapper mapper, IReservationService reservationService) 
        {
            _mapper = mapper;
            _reservationService = reservationService;
        }
        [HttpGet]
        public async Task<ActionResult> GetReservations()
        {
            var reservations = await _reservationService.GetReservationsAsync();
            if (reservations == null) 
                return NotFound(new ApiResponse<Reservation>(false,"Reservations Not Found"));
            var mappedReservations = _mapper.Map<List<ReservationGetDto>>(reservations);
            return Ok(new ApiResponse<List<ReservationGetDto>>(true,"Reservations Retrieved Succussfully"
                ,mappedReservations));
        }
        [Authorize(Roles ="Admin,Provider")]
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
        public async Task<IActionResult> MakeReservation(int clientId, int providerId,
           [FromBody] ReservationPostPutDto reservationPostPut)
        {
            
            var domainReservation = _mapper.Map<Reservation>(reservationPostPut);
            
            //if (!ModelState.IsValid)
            //{
            //    var errors = ModelState.ToDictionary(

            //        key => key.Key,
            //        value => value.Value.Errors.Select(e=> e.ErrorMessage).ToArray()
            //        );
            //    //return BadRequest(new ApiResponse<string>(false, "Validation Errors",errors));
            
            //}
            domainReservation.ClientId = clientId;
            domainReservation.ProviderId = providerId;
            domainReservation.Status = ReservationStatus.Pending;

            await _reservationService.MakeReservationAsync(domainReservation);

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
