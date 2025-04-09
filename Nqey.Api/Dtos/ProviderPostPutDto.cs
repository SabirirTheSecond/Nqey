using Nqey.Domain.Common;
using Nqey.Domain;
using Microsoft.AspNetCore.Identity;

namespace Nqey.Api.Dtos
{
    public class ProviderPostPutDto
    {
        //public int ProviderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? IdentityPiece { get; set; }
        public string ServiceDescription { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }

        

       
    }
}
