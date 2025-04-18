﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos;
using Nqey.DAL;
using AutoMapper;
using Nqey.Domain;
using Nqey.Domain.Common;
using Nqey.Domain.Abstractions.Repositories;
using Microsoft.AspNetCore.Authorization;
namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;


        public ServiceController(IServiceRepository serviceRepository,IUserRepository userRepository, IMapper mapper)
        {
            _serviceRepository = serviceRepository;
            _userRepository = userRepository;
            _mapper = mapper;


        }

        //[Authorize(Roles = "Client,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetServices()

        {
            var services = await _serviceRepository.GetServicesAsync();
            var servicesGet = _mapper.Map<List<ServiceGetDto>>(services);
            return Ok(new ApiResponse<List<ServiceGetDto>>(true, "Services List", servicesGet));
        }

        //[Authorize(Roles = "Client,Admin")]
        [Route("{serviceId}")]
        [HttpGet]
        public async Task<IActionResult> GetServiceById(int serviceId)
        {
            var service = await _serviceRepository.GetServiceByIdAsync(serviceId);
            var serviceGet = _mapper.Map<ServiceGetDto>(service);
            return Ok(new ApiResponse<ServiceGetDto>(true, "Services List", serviceGet));
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

            var serviceGet = _mapper.Map<ServiceGetDto>(domainService);

            return Ok(new ApiResponse<ServiceGetDto>(true, $"Service {serviceGet.Name} Added ", serviceGet));
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
                return NotFound(new ApiResponse<ServiceGetDto>(false, "Service not found", null));
            }

             _mapper.Map(servicePostPut, existingService);
           
            await _serviceRepository.UpdateServiceAsync(existingService);
            var mappedService = _mapper.Map<ServiceGetDto>(existingService);
            return Ok(new ApiResponse<ServiceGetDto>(true,$" Service is updated to {mappedService.Name}",null));


        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> DeleteService(int id)
        {
            var toDelete = await _serviceRepository.GetServiceByIdAsync(id);
            if (toDelete == null)
                return NotFound(new ApiResponse<ServiceGetDto>(false, "Service not found", null));

            await _serviceRepository.DeleteServiceAsync(id);

            return Ok(new ApiResponse<ServiceGetDto>(true,"Service Deleted",null));
        }

        [Authorize(Roles = "Client,Admin")]
        [HttpGet]
        [Route("{serviceId}/providers")]
        public async Task<ActionResult<List<Provider>>> GetProviders(int serviceId)
        {
            var providers = await _serviceRepository.GetAllProviderAsync(serviceId);
            var mappedProviders = _mapper.Map<List<ProviderGetDto>>(providers);
            var serviceName = await _serviceRepository.GetServiceByIdAsync(serviceId);
           
            // check the existence of the service
            if (serviceName == null)
                return NotFound(new ApiResponse<Provider>(false,"Service Not Found"));
            
            /* check if the providers list isn't empty and notify if otherwise*/
            if (providers == null)
                return Ok(new ApiResponse<List<ProviderGetDto>>(true, $" {serviceName.Name} has no providers yet", mappedProviders));

            return Ok(new ApiResponse<List<ProviderGetDto>>(true,$"List of {serviceName.Name} service providers",mappedProviders));

        }
        [Authorize(Roles = "Client,Admin")]
        [HttpGet]
        [Route("{serviceId}/providers/{providerId}")]
        public async Task<IActionResult> GetProviderById(int serviceId, int providerId)
        {
            var provider = await _serviceRepository.GetProviderByIdAsync(serviceId, providerId);
            if (provider == null)
                return NotFound(new ApiResponse<ProviderGetDto>(false, "provider not found", null));

            var mappedProvider = _mapper.Map<ProviderGetDto>(provider);

            return Ok(new ApiResponse<ProviderGetDto>(true, "Provider retrieved successfully",mappedProvider));

        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("{serviceId}/providers")]
        public async Task<IActionResult> AddProvider(int serviceId, [FromBody] ProviderPostPutDto providerPostPut)
        {
            var domainProvider = _mapper.Map<Provider>(providerPostPut);

            var userPostPut = _mapper.Map<UserPostPutDto>(providerPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Provider;
            domainUser.AccountStatus = AccountStatus.Blocked;

            domainProvider.SetPassword(providerPostPut.Password);
            await _serviceRepository.AddProviderAsync(serviceId, domainProvider);
            await _userRepository.AddUserAsync(domainUser);
            var mappedProvider = _mapper.Map<ProviderGetDto>(domainProvider);

            return Ok(new ApiResponse<ProviderGetDto>(true, "Provider Added Successfully", mappedProvider));
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
            return Ok(new ApiResponse<ProviderGetDto>(true, $"Provider {provider.UserName}'s Account Is Now Active"));

        }

    }
}
