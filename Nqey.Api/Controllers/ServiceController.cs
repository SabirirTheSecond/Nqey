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


        public ServiceController(IServiceRepository serviceRepository,IUserRepository userRepository
            , IMapper mapper, IClientRepository clientRepository, IImageUploaderService imageUploader)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _clientRepository = clientRepository;
            _mapper = mapper;
            _imageUploader = imageUploader;


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

            string? imagePath = null;

            if (servicePostPut.Image != null)
            {
                try
                {
                    imagePath = await _imageUploader.UploadImageToSupabase(servicePostPut.Image);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<string>(false, "Failed to upload image to Supabase", ex.Message));
                }
            }


            var domainService = _mapper.Map<Service>(servicePostPut);

            await _serviceRepository.AddServiceAsync(domainService);

            if (imagePath != null)
            {


                domainService.ServiceImage = new Image
                {
                    ImagePath = imagePath 

                };
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

        [AllowAnonymous]
        [HttpGet]
        [Route("providers")]
        public async Task<ActionResult> GetAllProviders()
        {
            var providers = await _serviceRepository.GetAllProvidersAsync();

            
            /* check if the providers list isn't empty and notify if otherwise*/
            if (providers == null)
                return Ok(new ApiResponse<Provider>(true, $"Proivders list is empty "));


            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }
            var roleClaim = User.FindFirst(ClaimTypes.Role);
            Console.WriteLine($"roleClaim = {roleClaim}");

            var userIdClaim0 = User.FindFirst("userId")?.Value;
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


                            Console.WriteLine($"Inside the nullable client id: {clientIdNullable}");
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
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, $"List of all providers", mappedProviders));

        }
        //[Authorize(Roles = "Client,Admin,Provider")]
        [AllowAnonymous]
        [HttpGet]
        [Route("{serviceId}/providers/{providerId}")]
        public async Task<IActionResult> GetServiceProviderById( int providerId)
        {
            var provider = await _serviceRepository.GetProviderByIdAsync( providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "provider not found", null));

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider retrieved successfully",mappedProvider));

        }
        [AllowAnonymous]
        [HttpGet]
        [Route("providers/{providerId}")]
        public async Task<IActionResult> GetProviderById(int providerId)
        {
            var provider = await _serviceRepository.GetProviderByIdAsync(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "provider not found", null));

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider retrieved successfully", mappedProvider));

        }

        //[Authorize(Roles = "Admin,Provider")]
        [HttpPost]
        [Route("{serviceId}/providers")]
        public async Task<IActionResult> AddProvider(int serviceId, [FromForm] ProviderPostPutDto providerPostPut)
        {
            string? imagePath = null;

            if (providerPostPut.ProfileImage != null)
            {
                try
                {
                    imagePath = await _imageUploader.UploadImageToSupabase(providerPostPut.ProfileImage);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<string>(false, "Failed to upload image to Supabase", ex.Message));
                }
               
            }
            
            var domainProvider = _mapper.Map<Provider>(providerPostPut);

            var userPostPut = _mapper.Map<UserPostPutDto>(providerPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Provider;
            domainUser.AccountStatus = AccountStatus.Blocked;
            domainUser.PhoneNumber = providerPostPut.PhoneNumber;

            domainProvider.SetPassword(providerPostPut.Password);
            await _serviceRepository.AddProviderAsync(serviceId, domainProvider);
            await _userRepository.AddUserAsync(domainUser);

            if (imagePath != null)
            {
                
                domainProvider.ProfileImage = new ProfileImage
                {

                    ImagePath = imagePath,
                    UserId = domainUser.UserId // If you generate ID before save, otherwise leave out and EF will link after

                };


            }
            //Uploading Portfolio images:
            if (providerPostPut.Portfolio != null && providerPostPut.Portfolio.Any())
            {
                domainProvider.Portfolio = new List<PortfolioImage>();
                foreach (var file in providerPostPut.Portfolio)
                {
                    try
                    {
                        imagePath = await _imageUploader.UploadImageToSupabase(file);
                        domainProvider.Portfolio.Add(new PortfolioImage
                        {
                            ImagePath = imagePath,
                            ProviderId = domainProvider.ProviderId
                        });
                    }
                    catch (Exception ex)
                    {
                        return StatusCode(500, new ApiResponse<string>(false, "Failed to upload portfolio image to Supabase", ex.Message));

                    }
                }
            }

            await _serviceRepository.UpdateProviderAsync(serviceId,domainProvider.ProviderId,domainProvider);

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

            var provider = await _serviceRepository.GetProviderByIdAsync(providerId);
            
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

            var existingProvider = await _serviceRepository.GetProviderByIdAsync(providerId);

            _mapper.Map(providerPostPut, existingProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(existingProvider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Updated Successfully", mappedProvider));


        }

    }
}
