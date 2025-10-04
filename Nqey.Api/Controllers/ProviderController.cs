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

       

        // Validate a provider's account
        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{userId}/ActivateProvider")]

        public async Task<IActionResult> ActivateProviderAccount( int userId)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(userId);
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
        [HttpPatch]
        [Route("edit")]
        public async Task<IActionResult> EditProvider([FromForm] ProviderPatchDto providerPatchDto)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<Object>(false, "Could Not Determine User Identity, Please Log In" +
                    "Again"));
            }
            

            var existingProvider = await _providerRepository.GetProviderByIdAsync(userId);
            _mapper.Map(providerPatchDto, existingProvider);

            var idImagePath = providerPatchDto.IdentityPiece != null
                 ? await _imageUploader.UploadImageToSupabase(providerPatchDto.IdentityPiece)
                 : existingProvider.IdentityPiece?.ImagePath;

            var selfiPath = providerPatchDto.SelfieImage != null
                ? await _imageUploader.UploadImageToSupabase(providerPatchDto.SelfieImage)
                : existingProvider.SelfieImage?.ImagePath;


            if (!existingProvider.IsIdentityVerified && idImagePath != null && selfiPath != null)
            {
                var isMatch = await _faceRecognitionService.VerifyFacesAsync(idImagePath, selfiPath);
                existingProvider.IsIdentityVerified = isMatch;
            }

            // Profile Image
            if (providerPatchDto.ProfileImage != null)
            {
                existingProvider.ProfileImage = await _imageService.UploadImageSafe(
                    providerPatchDto.ProfileImage, existingProvider.UserId);
            }

            // Portfolio Images
            if (providerPatchDto.Portfolio != null && providerPatchDto.Portfolio.Any())
            {
                existingProvider.Portfolio = await _imageService.UploadPortfolioImages(
                    providerPatchDto.Portfolio, existingProvider.UserId);
            }

            // IdentityPiece
            if (providerPatchDto.IdentityPiece != null)
            {
                existingProvider.IdentityPiece = new Image
                {
                    ImagePath = idImagePath,
                };
            }

            // SelfieImage
            if (providerPatchDto.SelfieImage != null)
            {
                existingProvider.SelfieImage = new Image
                {
                    ImagePath = selfiPath,
                };
            }
            await _serviceRepository.UpdateProviderAsync(existingProvider.ServiceId, existingProvider);
            
            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(existingProvider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Updated Successfully", mappedProvider));


        }

        [HttpPatch]
        [Route("upload-portfolio")]
        public async Task<IActionResult> UpdatePortfolio(List<IFormFile> portfolio)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "Could Not Determine User Identity, Please Log In" +
                    "Again"));
            }
            var provider = await _providerRepository.GetProviderByIdAsync(userId);
            if (provider == null)
            {
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "Provider Not Found"));
            }
            if (portfolio == null || !portfolio.Any())
            {
                return BadRequest(new ApiResponse<ProviderPublicGetDto>(false, "Please Add At Least One Image"));
               
            }
           var portfolioImages= await _imageService.UploadPortfolioImages(
                      portfolio, provider.UserId);
            await _providerRepository.UpdatePortfolio(userId, portfolioImages);
            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Portfolio Uploaded Successfully",mappedProvider));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("providers/{providerId}")]
        public async Task<IActionResult> GetProviderById(int providerId)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "provider not found", null));

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider retrieved successfully", mappedProvider));

        }
        
        [HttpPost]
        [Route("providers")]
        public async Task<IActionResult> AddProvider( [FromForm] ProviderPostPutDto providerPostPut)
        {

            var idImagePath = providerPostPut.IdentityPiece != null ?
                 await _imageUploader.UploadImageToSupabase(providerPostPut.IdentityPiece)
                 : null;

            var selfiPath = providerPostPut.SelfieImage != null ?
                
                await _imageUploader.UploadImageToSupabase(
                providerPostPut.SelfieImage)
                : null
                ;
            var isMatch = false;
            if (idImagePath != null && selfiPath != null)
            {
                 isMatch = await _faceRecognitionService.VerifyFacesAsync(idImagePath, selfiPath);
            }

            var domainProvider = _mapper.Map<Provider>(providerPostPut);
            domainProvider.SetPassword(providerPostPut.Password);

            if (isMatch) domainProvider.IsIdentityVerified = true;
            Console.WriteLine($"Identity  verified ? :{isMatch}");

            await _serviceRepository.AddProviderAsync(providerPostPut.ServiceId, domainProvider);         
            if (providerPostPut.ProfileImage != null)
            {

                domainProvider.ProfileImage = await _imageService.UploadImageSafe(
                    providerPostPut.ProfileImage, domainProvider.UserId);

            }
            //Uploading Portfolio images:
            if (providerPostPut.Portfolio != null && providerPostPut.Portfolio.Any())
            {
                domainProvider.Portfolio = await _imageService.UploadPortfolioImages(providerPostPut.Portfolio, domainProvider.UserId);
               

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


            await _serviceRepository.UpdateProviderAsync(providerPostPut.ServiceId, domainProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Added Successfully", mappedProvider));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("providers")]
        public async Task<IActionResult> GetAllProviders()
        {
            var providers = await _providerRepository.GetAllProvidersAsync();


            
            if (providers == null)
                return Ok(new ApiResponse<Provider>(true, $"Proivders list is empty "));


            var roleClaim = User.FindFirst(ClaimTypes.Role);
            Console.WriteLine($"roleClaim = {roleClaim}");

            var userIdClaim0 = User.FindFirst("userId")?.Value;
            Console.WriteLine($"userIdClaim0 = {userIdClaim0}");

            var role = roleClaim?.Value;
            Console.WriteLine($"before if statement , role = {role}");

            var userIdClaim = User.FindFirst("userId")?.Value;
            if(!int.TryParse(userIdClaim, out var userId) )
            {
                    
            }
            var user = await _userRepository.GetByIdAsync(userId);
            if (role == "Client")
            {

                
                Console.WriteLine($"Inside the if role part and userIdClaim= {userIdClaim}");
                
                     
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
                        providers = _recommendationService.GetSortedProvidersForClients(client, providers)
                            .Where(p=>p.AccountStatus != AccountStatus.Blocked)
                            .ToList();
                    }
                }
            
            var mappedProviders = _mapper.Map<List<ProviderPublicGetDto>>(providers);
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, $"List of all providers", mappedProviders));

        }
        // For Admin use:
        [Authorize(Roles = ("Admin"))]
        //[Authorize(Policy ="IsOwner")] IsOwner is only for reservations for the moment
        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateProvider(int id,[FromForm] ProviderAdminPatchDto providerPatchDto)
        {
           
            var existingProvider = await _serviceRepository.GetProviderByIdAsync(id);
            _mapper.Map(providerPatchDto, existingProvider);

            var idImagePath = providerPatchDto.IdentityPiece != null
                 ? await _imageUploader.UploadImageToSupabase(providerPatchDto.IdentityPiece)
                 : existingProvider.IdentityPiece?.ImagePath;

            var selfiPath = providerPatchDto.SelfieImage != null
                ? await _imageUploader.UploadImageToSupabase(providerPatchDto.SelfieImage)
                : existingProvider.SelfieImage?.ImagePath;


            if (!existingProvider.IsIdentityVerified && idImagePath != null && selfiPath != null)
            {
                var isMatch = await _faceRecognitionService.VerifyFacesAsync(idImagePath, selfiPath);
                existingProvider.IsIdentityVerified = isMatch;
            }

            // Profile Image
            if (providerPatchDto.ProfileImage != null)
            {
                existingProvider.ProfileImage = await _imageService.UploadImageSafe(
                    providerPatchDto.ProfileImage, existingProvider.UserId);
            }

            // Portfolio Images
            if (providerPatchDto.Portfolio != null && providerPatchDto.Portfolio.Any())
            {
                existingProvider.Portfolio = await _imageService.UploadPortfolioImages(
                    providerPatchDto.Portfolio, existingProvider.UserId);
            }

            // IdentityPiece
            if (providerPatchDto.IdentityPiece != null)
            {
                existingProvider.IdentityPiece = new Image
                {
                    ImagePath = idImagePath,
                };
            }

            // SelfieImage
            if (providerPatchDto.SelfieImage != null)
            {
                existingProvider.SelfieImage = new Image
                {
                    ImagePath = selfiPath,
                };
            }
            await _serviceRepository.UpdateProviderAsync(existingProvider.ServiceId, existingProvider);


            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(existingProvider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Updated Successfully", mappedProvider));


        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("{providerId}/statistics")]
        public async Task<IActionResult> GetProviderStatistics(int providerId)
        {
            var provider = await _providerRepository.GetProviderByIdAsync(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderPublicGetDto>(false, "Provider Not Found"));
            var statistics = provider.ProviderAnalytics;
            //var mappedStatistics = _mapper.Map<An>
            return Ok(new ApiResponse<ProviderAnalytics>(true, $"Provider {provider.UserName}'s " +
                $"Analytics", statistics));
        }
    }

}
