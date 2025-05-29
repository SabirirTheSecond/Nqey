using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Authorization;

namespace Nqey.Services.Authorization
{
    // This class serves as an ownership-based guard to a reservation ressource 
    public class IsOwnerHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        private readonly IReservationService _reservationService;
        private readonly IServiceRepository _serviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IClientRepository _clientRepo;
        public IsOwnerHandler(IServiceRepository serviceRepo, IReservationService reservationService,
            IUserRepository userRepo, IClientRepository clientRepo)
        {
            _serviceRepo = serviceRepo;
            _reservationService = reservationService;
            _userRepo = userRepo;
            _clientRepo = clientRepo;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsOwnerRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("userId");
            if (userIdClaim == null)
            {
                Console.WriteLine("ActiveAccount policy failed: No NameIdentifier claim found");
                Console.WriteLine($"userIdClaim : {userIdClaim}");
                context.Fail();
                return;
            }
            var userId = int.Parse(userIdClaim.Value);
            Console.WriteLine($"userId : {userId}");
            var user = await _userRepo.GetByIdAsync(userId);
            if( !(context.Resource is HttpContext httpContext))
            {
                Console.WriteLine($"Http Context stuff {context.Resource}");
                context.Fail();
                return ;
            }
            
            if( user.UserRole== Role.Provider)
            {
                // Change this later to a getProviderByUsername... Done
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
                    Console.WriteLine($"IsOwner Policy Succeeded for providerId: {providerId}");
                    context.Succeed(requirement);

                }

            }
            else if (user.UserRole == Role.Client)
            {
                var clientId = await _clientRepo.GetClientIdByUserNameAsync(user.UserName);
                var reservationId = httpContext.Request.RouteValues["id"]?.ToString();
                if (string.IsNullOrEmpty(reservationId) || !int.TryParse(reservationId, out var resId))
                {
                    Console.WriteLine($"Invalid reservationId: {reservationId}");
                    context.Fail();
                    return;
                }
                var reservation = await _reservationService.GetReservationByIdAsync(resId);
                if (reservation.ClientId == clientId)
                {
                    Console.WriteLine($"IsOwner Policy Succeeded for clientId: {clientId},");
                    context.Succeed(requirement);

                }
                

            }
            else if(user.UserRole == Role.Admin)
            {
                Console.WriteLine($"IsOwner Policy Succeeded for Admin");
                context.Succeed(requirement);

            }


        }
    }
}
