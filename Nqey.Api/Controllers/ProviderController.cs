using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ProviderController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IImageUploaderService _imageUploader;

        public ProviderController(IServiceRepository serviceRepository, IUserRepository userRepository
            , IMapper mapper, IImageUploaderService imageUploader)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _imageUploader = imageUploader;

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
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>(true, "List Of Pre-registered providers",mappedProviders));
        }

        [HttpGet]
        [Route("providerId")]
        
        public async Task<IActionResult> GetPreRegisteredProviderById(int providerId)
        {
            var provider = await _serviceRepository.GetPreRegisteredProviderById(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<Provider>(false, "Provider Not Found"));
            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(provider);
            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider", mappedProvider));
        }
        [HttpPost]
        
        public async Task<IActionResult> PreRegister([FromForm] ProviderPostPutDto providerPostPut)
        {
            string? imagePath = null;
            var serviceId = 35;

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

            await _serviceRepository.UpdateProviderAsync(serviceId, domainProvider.ProviderId, domainProvider);

            var mappedProvider = _mapper.Map<ProviderPublicGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderPublicGetDto>(true, "Provider Added Successfully", mappedProvider));
        }
    }
    
}
