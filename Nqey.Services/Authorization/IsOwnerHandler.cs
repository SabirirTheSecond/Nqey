using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Authorization;

namespace Nqey.Services.Authorization
{
    public class IsOwnerHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        private readonly IReservationService _reservationService;
        private readonly IServiceRepository _serviceRepo;
        private readonly IUserRepository _userRepo;

        public IsOwnerHandler(IServiceRepository serviceRepo, IReservationService reservationService,
            IUserRepository userRepo)
        {
            _serviceRepo = serviceRepo;
            _reservationService = reservationService;
            _userRepo = userRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                Console.WriteLine("ActiveAccount policy failed: No NameIdentifier claim found");
                context.Fail();
                return;
            }
            var userId = int.Parse(userIdClaim.Value);
            var user = await _userRepo.GetByIdAsync(userId);
            if( !(context.Resource is HttpContext httpContext))
            {
                Console.WriteLine($"Http Context stuff {context.Resource}");
                context.Fail();
                return ;
            }
            
            // Change this later to a getProviderByUsername
            var providerId = await _serviceRepo.GetProviderIdByUserNameAsync(user.UserName);
            var reservationId = httpContext.Request.RouteValues["id"]?.ToString();
            if (string.IsNullOrEmpty(reservationId) || !int.TryParse(reservationId, out var resId))
            {
                Console.WriteLine($"Invalid reservationId: {reservationId}");
                context.Fail();
                return;
            }
            var reservation = await _reservationService.GetReservationByIdAsync(resId);
            if (reservation.ProviderId == providerId)
            {
                Console.WriteLine($"IsOwner Policy Succeeded for {reservation.ReservationId},");
                context.Succeed(requirement);

            }

        }
    }
}
