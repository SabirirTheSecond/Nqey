using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos;
using Nqey.DAL;
using AutoMapper;
using Nqey.Domain;
using Nqey.Domain.Common;
using Nqey.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Api.Dtos.ServiceDtos;
using Nqey.Api.Dtos.ClientDtos;
using System.Security.Claims;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Helpers;
namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository ;
        private readonly IImageUploaderService _imageUploader;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;


        public ServiceController(IServiceRepository serviceRepository,IUserRepository userRepository
            , IMapper mapper, IClientRepository clientRepository,
            IImageUploaderService imageUploader, IImageService imageService)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _imageUploader = imageUploader;
            _imageService = imageService;


        }

        //[Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetServices()

        {
            var services = await _serviceRepository.GetServicesAsync();
            if (services == null)
            {
                return NotFound(new ApiResponse<List<ServicePublicGetDto>>(false, "Services List Is Empty"));
            }
            var servicesGet = _mapper.Map<List<ServicePublicGetDto>>(services);
            return Ok(new ApiResponse<List<ServicePublicGetDto>>(true, "Services List", servicesGet));
        }
        [HttpGet("service-name")]
        public async Task<IActionResult> GetServiceByName(string serviceName)
        {
            var service = await _serviceRepository.GetServiceByServiceName(serviceName);
            if (service == null) {
                return NotFound(new ApiResponse<Service>(false, "Service Not Found"));
            }
            var mappedService = _mapper.Map<ServicePublicGetDto>(service);
            return Ok(new ApiResponse<ServicePublicGetDto>(true, $"Service {mappedService.NameEn}", mappedService));
        }
                //[Authorize(Roles = "Client,Admin")]
        [Route("{serviceId}")]
        [HttpGet]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
            var serviceGet = _mapper.Map<ServicePublicGetDto>(service);
            return Ok(new ApiResponse<ServicePublicGetDto>(true, "Services List", serviceGet));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddService([FromForm] ServicePostPutDto servicePostPut)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.ToDictionary(
                key => key.Key,
                value => value.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );
                return BadRequest(new ApiResponse<Dictionary<string, string[]>>(false, "Validation errors", errors));
            }

            

            var domainService = _mapper.Map<Service>(servicePostPut);

            await _serviceRepository.AddServiceAsync(domainService);

            if (servicePostPut.Image != null)
            {


                domainService.ServiceImage = await _imageService.UploadServiceImage(
                    servicePostPut.Image, domainService.ServiceId);

                await _serviceRepository.UpdateServiceAsync(domainService);

            }

            var serviceGet = _mapper.Map<ServicePublicGetDto>(domainService);

            return Ok(new ApiResponse<ServicePublicGetDto>(true, $"Service {serviceGet.NameEn} Added ", serviceGet));
            //return Ok(serviceGet);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateService(ServicePostPutDto servicePostPut, int id)
        {
            var existingService = await _serviceRepository.GetServiceByIdAsync(id);
           
            if (existingService == null)
            {
                return NotFound(new ApiResponse<ServicePublicGetDto>(false, "Service not found", null));
            }

             _mapper.Map(servicePostPut, existingService);
           
            await _serviceRepository.UpdateServiceAsync(existingService);
            var mappedService = _mapper.Map<ServicePublicGetDto>(existingService);
            return Ok(new ApiResponse<ServicePublicGetDto>(true,$" Service is updated to {mappedService.NameEn}",null));


        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteService(int id)
        {
            var toDelete = await _serviceRepository.GetServiceByIdAsync(id);
            if (toDelete == null)
                return NotFound(new ApiResponse<ServicePublicGetDto>(false, "Service not found", null));

            await _serviceRepository.DeleteServiceAsync(id);

            return Ok(new ApiResponse<ServicePublicGetDto>(true,"Service Deleted",null));
        }

        // This endpoint returns a given service providers
        [AllowAnonymous]
        [HttpGet]
        [Route("{serviceId}/providers")]
        public async Task<ActionResult> GetServiceProviders(int serviceId)
        {
            var providers = await _serviceRepository.GetProvidersByServicAsync(serviceId);
            
            var serviceName = await _serviceRepository.GetServiceByIdAsync(serviceId);
           

            // check the existence of the service
            if (serviceName == null)
                return NotFound(new ApiResponse<Provider>(false,"Service Not Found"));
            
            /* check if the providers list isn't empty and notify if otherwise*/
            if (providers == null)
                return Ok(new ApiResponse<Provider>(true, $" {serviceName.NameEn} has no providers yet"));


            foreach(var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            Console.WriteLine($"roleClaim = {roleClaim}");

            var userIdClaim0= User.FindFirst("userId")?.Value;
            Console.WriteLine($"userIdClaim0 = {userIdClaim0}");
            
            var role = roleClaim?.Value;
            Console.WriteLine($"before if statement , role = {role}");


            if (role == "Client")
            {

                var userIdClaim = User.FindFirst("userId")?.Value;
                Console.WriteLine($"Inside the if role part and userIdClaim= {userIdClaim}");
                if (int.TryParse(userIdClaim, out var userId))
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        Console.WriteLine($"Inside the if user!=null part and user= {user}");
                        var clientIdNullable = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
                        if (clientIdNullable is not int clientId)
                        {

                        
                        Console.WriteLine($"Inside the nullable client id: {clientIdNullable }");
                        return BadRequest(new ApiResponse<Reservation>(false, "Cannot determine client id"));
                        }
                        var client = await _clientRepository.GetClientByIdAsync(clientId);
                        if (client?.Location?.Position != null)
                        {

                            providers = providers
                                .Select(p => new { Provider = p, Score = ScoreCalculator.CalculateScore(client, p) })
                                .OrderByDescending(p => p.Score)
                                .Select(p => p.Provider)
                                .ToList();
                            Console.WriteLine($"We are inside the part that works {providers}");
                        }
                    }
                }
            }
            var mappedProviders = _mapper.Map<List<ProviderPublicGetDto>>(providers);
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true,$"List of {serviceName.NameEn} service providers",mappedProviders));

        }

    }
}
