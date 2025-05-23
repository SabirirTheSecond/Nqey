using Nqey.Domain.Common;
using Nqey.Domain;
using Nqey.Api.Dtos.ProfileImageDtos;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderPublicGetDto
    {

        public int ProviderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // you probably need to put it as ProfileImageDto 
        public ProfileImageGetDto? ProfileImage { get; set; }
        public List<PortfolioImageDto>? Portfolio { get; set; }
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
