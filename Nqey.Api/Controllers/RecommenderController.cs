using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos.ProviderDtos;
using Nqey.Api.Dtos.RecoDtos;
using Nqey.DAL;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;
using Nqey.Services.Services;


namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class RecommenderController(DataContext dataContext,
        IProviderRepository providerRepo, IMapper mapper) : Controller
    {
        [Authorize]
        [HttpGet("recommend_providers")]
        public async Task<IActionResult> GetRecommended([FromServices] ProviderRecommender recommender)
        {
            var clientIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(clientIdClaim, out var clientId))
            {
                return NotFound(new ApiResponse<Client>(false, "Please login before you contine"));
            }

            var pastProvidersIds = await dataContext.Reservations.Where(
                r=> r.ClientUserId == clientId && r.Status == ReservationStatus.Completed)
                .Select(r=>r.ProviderUserId)
                .Distinct()
                .ToListAsync()
                ;

            var providers = await providerRepo.GetAllProvidersAsync();
            int serviceCount = await dataContext.Services.CountAsync();
            int subserviceCount = await dataContext.SubServices.CountAsync();


            var req = new ProviderRecommenderRequestDto
            {
                Providers = providers.Select(p => new ProviderSendDto
                {
                    Id = p.UserId,
                    Service_Id = p.ServiceId,
                    Subservices = p.SubServices?.Select(s=>s.SubServiceId).ToList(),
                    Avg_Rating = p.Reviews.Any() ? p.AverageRating : 0,
                    Jobs_Done = p.ProviderAnalytics.JobsDone,
                }).ToList(),

                Client_History = new ClientHistorySendDto
                {
                    Provider_Ids = pastProvidersIds
                },
                Meta = new MetaSendDto
                {
                    Service_Count = serviceCount,
                    Subservice_Count = subserviceCount,
                }

            };

            var ranked = await recommender.RecommendProvidersAsync(req);
            var topIds = ranked.Take(6).ToList();

            var providersLookup = providers.ToDictionary(p => p.UserId, p => p);
            var topProviders = topIds
                .Where(id => providersLookup.ContainsKey(id))
                .Select(id=> providersLookup[id])
                .ToList();

            var mappedProviders = mapper.Map<List<ProviderPublicGetDto>>(topProviders);
            return Ok(new ApiResponse<List<ProviderPublicGetDto>>
                (true,"Suggested Providers  For you",mappedProviders));





        }
    }
}