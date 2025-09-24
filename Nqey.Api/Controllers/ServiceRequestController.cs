using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos.ServiceRequestDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;
namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestRepository _serviceRequestRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;


        public ServiceRequestController(IServiceRequestRepository serviceRepository,
            IMapper mapper, IUserRepository userRepository)
        {
            _serviceRequestRepo = serviceRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetServiceRequests()
        {
            var serviceRequests= await _serviceRequestRepo.GetAllServiceReqquestsAsync();
            if (serviceRequests == null)
            { 
                return Ok(new ApiResponse<ServiceRequest>(true, "No Service Requests"));
            }
            return Ok(new ApiResponse<List<ServiceRequest>>(true, "List Of Service Requests",serviceRequests));
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetServiceRequestById(int id)
        {
            var serviceRequest= await _serviceRequestRepo.GetServiceRequestByIdAsync(id);
            if (serviceRequest == null)
            {
                return NotFound(new ApiResponse<ServiceRequest>(false, "Service Request Not Found"));
            }
            var mappedRequest = _mapper.Map<ServiceRequestGetDto>(serviceRequest);

            return Ok(new ApiResponse<ServiceRequestGetDto>(true,"Service Request Retrieved", mappedRequest));

        }
        [Authorize(Roles =("Admin,Provider"))]
        [HttpPost]
        public async Task<IActionResult> AddServiceRequest(ServiceRequestPostPutDto serviceReqPostDto)
        {
            var userIdClaim = User.FindFirst("userId")?.Value;
            var domainServiceReq= _mapper.Map<ServiceRequest>(serviceReqPostDto);

            if(int.TryParse(userIdClaim, out var userId))
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if(user == null)
                {
                    return BadRequest(new ApiResponse<ServiceRequest>(false, "Invalid User"));
                }

            }
            domainServiceReq.ProviderUserId= userId;
            await _serviceRequestRepo.AddServiceRequestAsync(domainServiceReq, userId);
            var mappedServiceReq = _mapper.Map<ServiceRequestGetDto>(domainServiceReq);
            return Ok(new ApiResponse<ServiceRequestGetDto>(true, "Your New Service Request Is Sent", mappedServiceReq));

        }

    }
}
