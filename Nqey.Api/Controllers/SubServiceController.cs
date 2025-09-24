using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos.SubServiceDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class SubServiceController(ISubServiceRepository subServiceRepo,
        IProviderRepository providerRepo, IMapper mapper) : Controller
    {

        [Authorize(Roles ="Provider")]
        [HttpPost]
        public async Task<IActionResult> PostSubService(SubServicePostDto subServicePostDto)
        {

            var userIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<SubServiceGetDto>(false, "Could Not Determine User Identity, Please Log In" +
                    "Or Contact Support"));
            }
            var provider = await providerRepo.GetProviderByIdAsync(userId);
            var domainSubService = mapper.Map<SubService>(subServicePostDto);
            
            domainSubService.ProviderUserId = userId;
            await subServiceRepo.AddSubServiceAsync(domainSubService);
            var mappedSubService= mapper.Map<SubServiceGetDto>(domainSubService);
            return Ok(new ApiResponse<SubServiceGetDto>(true, "Sub Service Added Successfully", mappedSubService));
        }
        [Authorize(Roles ="Provider")]
        [HttpGet]
        [Route("provider-subservices")]
        public async Task<IActionResult> GetSubServicesByProvider()
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<SubServiceGetDto>(false, "Could Not Determine User Identity, Please Log In" +
                    " Or Contact Support"));
            }
            var provider = await providerRepo.GetProviderByIdAsync(userId);
          var subServices=  await subServiceRepo.GetSubServicesAsync(userId);
            var mappedSubServices = mapper.Map<List<SubServiceGetDto>>(subServices);
            return Ok(new ApiResponse<List<SubServiceGetDto>>(true, $"List Of Provider's {provider.UserName} Sub Services",
                mappedSubServices));
        }
        
        [HttpGet]
        [Route("provider-subservices/{providerId}")]
        public async Task<IActionResult> GetSubServicesByProviderId(int providerId)
        {
            
            var provider = await providerRepo.GetProviderByIdAsync(providerId);
            if (provider == null)
                return NotFound(new ApiResponse<SubService>(false, "Provider Not Found"));
            var subServices = await subServiceRepo.GetSubServicesAsync(providerId); 
            var mappedSubServices = mapper.Map<List<SubServiceGetDto>>(subServices);
            
            return Ok(new ApiResponse<List<SubServiceGetDto>>(true, $"List Of Provider's {provider.UserName} Sub Services",
                mappedSubServices));
        }
        [Authorize(Roles ="Provider")]
        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteSubService(int id)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return NotFound(new ApiResponse<SubServiceGetDto>(false, "Could Not Determine User Identity, Please Log In" +
                    " Or Contact Support"));
            }
            var provider = await providerRepo.GetProviderByIdAsync(userId);
            await subServiceRepo.DeleteSubServiceAsync(id);
            return Ok(new ApiResponse<List<SubServiceGetDto>>(true, $"Sub Service Deleted"));

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubserviceById(int id)
        {
            var subservice = await subServiceRepo.GetSubServiceByIdAsync(id);
            if (subservice == null)
                return NotFound(new ApiResponse<SubService>(false, "Sub Service Not Found"));
            var mappedSubService = mapper.Map<SubServiceGetDto>(subservice);
            return Ok(new ApiResponse<SubServiceGetDto>(true,"Sub Service Retrieved",mappedSubService));
        }

    }
}
