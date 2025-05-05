using Nqey.Domain.Common;
using Nqey.Domain;
using Nqey.Api.Dtos.ProfileImageDtos;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderAdminGetDto
    {
        public int ProviderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ProfileImageGetDto? ProfileImage { get; set; }
        public string IdentityPiece { get; set; }
        public string ServiceDescription { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public Location? Location { get; set; }
        public AccountStatus AccountStatus { get; set; } 
        public int ServiceId { get; set; }
      
        public virtual ICollection<Message> SentMessages { get; set; } 
        public virtual ICollection<Message> ReceivedMessages { get; set; } 
    }
}
