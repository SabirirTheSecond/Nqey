using Nqey.Domain.Common;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderPublicGetDto
    {
        public int ProviderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //public string? IdentityPiece { get; set; }

        public ProfileImage ProfilePicture { get; set; }
        public string ServiceDescription { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Location? Location { get; set; }
        //public AccountStatus AccountStatus { get; set; } = AccountStatus.Blocked;
        public int ServiceId { get; set; }
        //public Service Service { get; set; }
        //public Review? Review { get; set; }
        
    }
}
