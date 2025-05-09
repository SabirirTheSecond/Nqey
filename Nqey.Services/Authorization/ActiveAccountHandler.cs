using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Authorization;

namespace Nqey.Services.Authorization
{
    public class ActiveAccountHandler : AuthorizationHandler<ActiveAccountRequirement>
    {
        private readonly IUserRepository _userRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IClientRepository _clientRepo;

        public ActiveAccountHandler(IUserRepository userRepo, IServiceRepository serviceRepo,
            IClientRepository clientRepo)
        {
            _userRepo = userRepo;
            _serviceRepo = serviceRepo;
            _clientRepo = clientRepo;
            
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveAccountRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst("userId");
            if (userIdClaim == null)
            {
                Console.WriteLine("ActiveAccount policy failed: No userId claim found");
                context.Fail();
                return;
            }
            var userId = int.Parse(userIdClaim.Value);
            var user = await _userRepo.GetByIdAsync(userId);

            object fullUser = null;
            AccountStatus accountStatus = user.AccountStatus;
           
            switch (user.UserRole)
            {
                case Role.Provider:
                    fullUser = await _serviceRepo.GetProviderIdByUserNameAsync(user.UserName);
                    break;
                case Role.Client:
                    fullUser = await _clientRepo.GetClientIdByUserNameAsync(user.UserName);
                    break;
                default:
                    
                    throw new Exception("Unsupported role");
            }
            if(fullUser == null)
            {
                Console.WriteLine($"ActiveAccount policy failed: User ID {userId} not found", userId);
                context.Fail();
                return;
            }

            if(  accountStatus == Domain.AccountStatus.Active)
            {
                Console.WriteLine($"ActiveAccount policy succeeded for user ID {userId}");
                context.Succeed(requirement);
            }
            else
            {
                Console.WriteLine($"ActiveAccount policy failed: User ID {userId} has status {user.AccountStatus}");
                context.Fail();
            }



        }
        
    }
}
