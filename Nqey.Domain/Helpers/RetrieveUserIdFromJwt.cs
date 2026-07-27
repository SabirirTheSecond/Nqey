using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Helpers
{
    public class RetrieveUserIdFromJwt
    {
        public RetrieveUserIdFromJwt()
        {

        }
        public int RetrieveAuthenticatedUserId(Role role)
        {
            return 0;
        }

        //        var roleClaim = User.FindFirst(ClaimTypes.Role);


        //            var userIdClaim0 = User.FindFirst("userId")?.Value;


        //            var role = roleClaim?.Value;


        //            var userIdClaim = User.FindFirst("userId")?.Value;
        //            if(!int.TryParse(userIdClaim, out var userId) )
        //            {
        //                return NotFound(new ApiResponse<ProviderAdminGetDto>(false, "Not Found"));   
        //            }
        //}
    }
}
