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
    public class IsReservationOwnerHandler : AuthorizationHandler<IsOwnerRequirement>
    {
        private readonly IReservationService _reservationService;
        private readonly IServiceRepository _serviceRepo;
        private readonly IUserRepository _userRepo;
        private readonly IClientRepository _clientRepo;
        public IsReservationOwnerHandler(IServiceRepository serviceRepo, IReservationService reservationService,
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
           
            var routeValues = httpContext.Request.RouteValues;
           //Case 1: If the route has reservation id :
            if (routeValues.TryGetValue("id", out var reservationIdObj) 
                && int.TryParse(reservationIdObj?.ToString(), out var reservationId))
            {

                var reservation = await _reservationService.GetReservationByIdAsync(reservationId);
                if(reservation == null)
                {
                    Console.WriteLine($"Invalid reservationId: {reservationId}");
                    context.Fail();
                    return ;
                }
                
               

                
                    if(reservation?.ProviderUserId == userId || reservation?.ClientUserId == userId
                    || user.UserRole== Role.Admin)
                    {
                        context.Succeed(requirement);
                    return;
                    }
              context.Fail();
                return ;
            }

          
            
            
        }
    }
}
