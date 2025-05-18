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
        private readonly IMapper _mapper;


        public ServiceController(IServiceRepository serviceRepository,IUserRepository userRepository
            , IMapper mapper, IClientRepository clientRepository)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;


        }

        //[Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetServices()

        {
            var services = await _serviceRepository.GetServicesAsync();
            var servicesGet = _mapper.Map<List<ServicePublicGetDto>>(services);
            return Ok(new ApiResponse<List<ServicePublicGetDto>>(true, "Services List", servicesGet));
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
        public async Task<IActionResult> AddService([FromBody] ServicePostPutDto servicePostPut)
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

            var serviceGet = _mapper.Map<ServicePublicGetDto>(domainService);

            return Ok(new ApiResponse<ServicePublicGetDto>(true, $"Service {serviceGet.Name} Added ", serviceGet));
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
            return Ok(new ApiResponse<ServicePublicGetDto>(true,$" Service is updated to {mappedService.Name}",null));


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

        [Authorize(Roles = "Client,Admin,Provider")]
        [HttpGet]
        [Route("{serviceId}/providers")]
        public async Task<ActionResult> GetProviders(int serviceId)
        {
            var providers = await _serviceRepository.GetAllProviderAsync(serviceId);
            var mappedProviders = _mapper.Map<List<ProviderPublicGetDto>>(providers);
            var serviceName = await _serviceRepository.GetServiceByIdAsync(serviceId);
           

            // check the existence of the service
            if (serviceName == null)
                return NotFound(new ApiResponse<Provider>(false,"Service Not Found"));
            
            /* check if the providers list isn't empty and notify if otherwise*/
            if (providers == null)
                return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, $" {serviceName.Name} has no providers yet", mappedProviders));


            var role = User.FindFirst("role")?.Value;

            if (role == "Client")
            {
                var userIdClaim = User.FindFirst("userId")?.Value;

                if (int.TryParse(userIdClaim, out var userId))
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null)
                    {
                        var clientIdNullable = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
                        if (clientIdNullable is not int clientId)
                            return BadRequest(new ApiResponse<Reservation>(false, "Cannot determine client id"));

                        var client = await _clientRepository.GetClientByIdAsync(clientId);
                        if (client?.Location?.Position != null)
                        {
                            providers = providers
                                .Select(p => new { Provider = p, Score = ScoreCalculator.CalculateScore(client, p) })
                                .OrderByDescending(p => p.Score)
                                .Select(p => p.Provider)
                                .ToList();
                        }
                    }
                }
            }

                return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true,$"List of {serviceName.Name} service providers",mappedProviders));

        }
        [Authorize(Roles = "Client,Admin,Provider")]
        [HttpGet]
        [Route("{serviceId}/providers/{providerId}")]
        public async Task<IActionResult> GetProviderById(int serviceId, int providerId)
        {
            var provider = await _serviceRepository.GetProviderByIdAsync(serviceId, providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "provider not found", null));

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider retrieved successfully",mappedProvider));

        }
        //[Authorize(Roles = "Admin,Provider")]
        [HttpPost]
        [Route("{serviceId}/providers")]
        public async Task<IActionResult> AddProvider(int serviceId, [FromForm] ProviderPostPutDto providerPostPut)
        {
            string? imagePath = null;

            if (providerPostPut.ProfileImage != null)
            {
                Console.WriteLine($"Profile image: {providerPostPut.ProfileImage} not null");

                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(providerPostPut.ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await providerPostPut.ProfileImage.CopyToAsync(stream);
                }

                imagePath = Path.Combine("images", "profiles", uniqueFileName);
            }
            else Console.WriteLine($"Profile image: {providerPostPut.ProfileImage} is null");

            var domainProvider = _mapper.Map<Provider>(providerPostPut);

            var userPostPut = _mapper.Map<UserPostPutDto>(providerPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Provider;
            domainUser.AccountStatus = AccountStatus.Blocked;

            domainProvider.SetPassword(providerPostPut.Password);
            await _serviceRepository.AddProviderAsync(serviceId, domainProvider);
            await _userRepository.AddUserAsync(domainUser);

            if (imagePath != null)
            {
                Console.WriteLine($"image path: {imagePath} not null");
                domainProvider.ProfileImage = new ProfileImage
                {

                    ImagePath = imagePath,
                    UserId = domainUser.UserId // If you generate ID before save, otherwise leave out and EF will link after

                };

                await _serviceRepository.UpdateProviderAsync(serviceId,domainProvider.ProviderId,domainProvider);

            }
            else Console.WriteLine($" image path: {imagePath} is null");
            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Added Successfully", mappedProvider));
        }
        // Validate a provider's account
        [Authorize(Roles ="Admin")]
        [HttpPut]
        [Route("{serviceId}/providers/{providerId}/ActivateProvider")]

        public async Task<IActionResult> ActivateProviderAccount(int serviceId, int providerId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
            if (service == null)
                return NotFound(new ApiResponse<Service>(false,"Service Not Found"));

            var provider = await _serviceRepository.GetProviderByIdAsync(serviceId,providerId);
            
            if(provider == null)
                return NotFound(new ApiResponse<Provider>(false, "Provider Not Found"));

            await _serviceRepository.ActivateProviderAsync(serviceId, provider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, $"Provider {provider.UserName}'s Account Is Now Active"));

        }


        [Authorize(Roles =("Provider,Admin"))]
        //[Authorize(Policy ="IsOwner")] IsOwner is only for reservations for the moment
        [HttpPut]
        [Route("{serviceId}/providers/{providerId}/update")]
        public async Task<IActionResult> UpdateProvider(int serviceId,int providerId,[FromForm] ProviderPostPutDto providerPostPut)
        {
            var service = _serviceRepository.GetServiceByIdAsync(serviceId);
            
            if (service == null)
                return NotFound(new ApiResponse<Service>(false, "Service Not Found"));

            var existingProvider = await _serviceRepository.GetProviderByIdAsync(serviceId, providerId);
            _mapper.Map(providerPostPut, existingProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(existingProvider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Updated Successfully", mappedProvider));


        }

    }
}
