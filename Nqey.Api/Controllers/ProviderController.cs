using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.DAL.Repositories;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;
using Nqey.Domain.Helpers;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProviderController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProviderRepository _providerRepository;
        private readonly IMapper _mapper;
        private readonly IImageUploaderService _imageUploader;
        private readonly IImageService _imageService;
        private readonly IRecommendationService _recommendationService;
        private readonly IFaceRecognitionService _faceRecognitionService;
        public ProviderController(IServiceRepository serviceRepository, IUserRepository userRepository
            , IMapper mapper, IImageUploaderService imageUploader, 
            IClientRepository clientRepository, IImageService imageService,
            IProviderRepository providerRepository, 
            IRecommendationService recommendationService, IFaceRecognitionService faceRecognition)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _providerRepository = providerRepository;
            _mapper = mapper;
            _imageUploader = imageUploader;
            _clientRepository = clientRepository;
           _recommendationService = recommendationService;
            _imageService = imageService;
            _faceRecognitionService = faceRecognition;
        }

        [HttpGet]
        public async Task<IActionResult> GetPreRegisteredProviders()
        {
            var providers = await _serviceRepository.GetPreRegisteredProviders();
            if (!providers.Any())
            {
                return NotFound(new ApiResponse<Provider>(false, "Providers Null Or Not Found"));
            }
            var mappedProviders = _mapper.Map<List<ProviderPublicGetDto>>(providers);
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, "List Of Pre-registered providers", mappedProviders));
        }

        [HttpGet]
        [Route("${providerId}/preRegisteredProvider")]

        public async Task<IActionResult> GetPreRegisteredProviderById(int providerId)
        {
            var provider = await _serviceRepository.GetPreRegisteredProviderById(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<Provider>(false, "Provider Not Found"));
            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider", mappedProvider));
        }
        [HttpPost]
        [Route("PreRegister")]
        public async Task<IActionResult> PreRegister([FromForm] ProviderPostPutDto providerPostPut)
        {
            
            var serviceId = 35;

            var idImagePath = await _imageUploader.UploadImageToSupabase(
                providerPostPut.IdentityPiece);

            var selfiPath = await _imageUploader.UploadImageToSupabase(
                providerPostPut.SelfieImage);

           
            var isMatch = await _faceRecognitionService.VerifyFacesAsync(idImagePath, selfiPath);

            
            var domainProvider = _mapper.Map<Provider>(providerPostPut);

            var userPostPut = _mapper.Map<UserPostPutDto>(providerPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Provider;
            domainUser.AccountStatus = AccountStatus.Blocked;
            domainUser.PhoneNumber = providerPostPut.PhoneNumber;

            domainProvider.SetPassword(providerPostPut.Password);
            if(isMatch) domainProvider.IsIdentityVerified = true;
            Console.WriteLine($"Identity  verified ? :{isMatch}");

            await _serviceRepository.AddProviderAsync(serviceId, domainProvider);
            await _userRepository.AddUserAsync(domainUser);

            // uploads the profile image
            if (providerPostPut.ProfileImage != null)
            {

                domainProvider.ProfileImage = await _imageService.UploadImageSafe(
                    providerPostPut.ProfileImage, domainUser.UserId);

            }
            //Uploads portfolio images:
            if (providerPostPut.Portfolio != null && providerPostPut.Portfolio.Any())
            {
                domainProvider.Portfolio = await _imageService.UploadPortfolioImages(providerPostPut.Portfolio, domainProvider.ProviderId);

            }

            await _serviceRepository.UpdateProviderAsync(serviceId, domainProvider.ProviderId, domainProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Added Successfully", mappedProvider));
        }


        // Validate a provider's account
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{providerId}/ActivateProvider")]

        public async Task<IActionResult> ActivateProviderAccount( int providerId)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(providerId);
            var service = await _serviceRepository.GetServiceByIdAsync(provider.ServiceId);
            if (service == null)
                return NotFound(new ApiResponse<Service>(false, "Service Not Found"));

            if (provider == null)
                return NotFound(new ApiResponse<Provider>(false, "Provider Not Found"));

            await _serviceRepository.ActivateProviderAsync( provider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, $"Provider {provider.UserName}'s Account Is Now Active"));

        }

        [Authorize(Roles = ("Provider,Admin"))]
        //[Authorize(Policy ="IsOwner")] IsOwner is only for reservations for the moment
        [HttpPut]
        [Route("{serviceId}/providers/{providerId}/update")]
        public async Task<IActionResult> UpdateProvider(int serviceId, int providerId, [FromForm] ProviderPostPutDto providerPostPut)
        {

            var service = _serviceRepository.GetServiceByIdAsync(serviceId);

            if (service == null)
                return NotFound(new ApiResponse<Service>(false, "Service Not Found"));

            var existingProvider = await _serviceRepository.GetProviderByIdAsync(providerId);

            _mapper.Map(providerPostPut, existingProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(existingProvider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Updated Successfully", mappedProvider));


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
        // --- difference here is the service Id.. yes i should probably drop this endpoint
        //[Authorize(Roles = "Client,Admin,Provider")]
        //[AllowAnonymous]
        //[HttpGet]
        //[Route("{serviceId}/providers/{providerId}")]
        //public async Task<IActionResult> GetServiceProviderById(int providerId)
        //{
        //    var provider = await _serviceRepository.GetProviderByIdAsync(providerId);
        //    if (provider == null)
        //        return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "provider not found", null));

        //    var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);

        //    return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider retrieved successfully", mappedProvider));

        //}
        //[Authorize(Roles = "Admin,Provider")]
        [HttpPost]
        [Route("{serviceId}/providers")]
        public async Task<IActionResult> AddProvider(int serviceId, [FromForm] ProviderPostPutDto providerPostPut)
        {

            var idImagePath = await _imageUploader.UploadImageToSupabase(
               providerPostPut.IdentityPiece);

            var selfiPath = await _imageUploader.UploadImageToSupabase(
                providerPostPut.SelfieImage);

            var isMatch = await _faceRecognitionService.VerifyFacesAsync(idImagePath, selfiPath);

            var domainProvider = _mapper.Map<Provider>(providerPostPut);

            var userPostPut = _mapper.Map<UserPostPutDto>(providerPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Provider;
            domainUser.AccountStatus = AccountStatus.Blocked;
            domainUser.PhoneNumber = providerPostPut.PhoneNumber;
            domainProvider.SetPassword(providerPostPut.Password);

            if (isMatch) domainProvider.IsIdentityVerified = true;
            Console.WriteLine($"Identity  verified ? :{isMatch}");

            await _serviceRepository.AddProviderAsync(serviceId, domainProvider);
            await _userRepository.AddUserAsync(domainUser);

            if (providerPostPut.ProfileImage != null)
            {

                domainProvider.ProfileImage = await _imageService.UploadImageSafe(
                    providerPostPut.ProfileImage, domainUser.UserId);

            }
            //Uploading Portfolio images:
            if (providerPostPut.Portfolio != null && providerPostPut.Portfolio.Any())
            {
                domainProvider.Portfolio = await _imageService.UploadPortfolioImages(providerPostPut.Portfolio, domainProvider.ProviderId);
               

            }

            if( providerPostPut.IdentityPiece != null)
            {
                domainProvider.IdentityPiece = new Image
                {
                    ImagePath = idImagePath,
                };
            }
            if (providerPostPut.SelfieImage != null)
            {
                domainProvider.SelfieImage = new Image
                {
                    ImagePath = selfiPath,
                };
            }


            await _serviceRepository.UpdateProviderAsync(serviceId, domainProvider.ProviderId, domainProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Added Successfully", mappedProvider));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _providerRepository.GetAllProvidersAsync();


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
                        Console.WriteLine($"Inside the if 'user!=null' part and user= {user}");
                        var clientIdNullable = await _clientRepository.GetClientIdByUserNameAsync(user.UserName);
                        if (clientIdNullable is not int clientId)
                        {


                            Console.WriteLine($"Inside the nullable client id: {clientIdNullable}");
                            return BadRequest(new ApiResponse<Reservation>(false, "Cannot determine client id"));
                        }
                        var client = await _clientRepository.GetClientByIdAsync(clientId);
                        providers = _recommendationService.GetSortedProviders(client, providers);
                    }
                }
            }
            var mappedProviders = _mapper.Map<List<ProviderPublicGetDto>>(providers);
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, $"List of all providers", mappedProviders));

        }

    }

}
