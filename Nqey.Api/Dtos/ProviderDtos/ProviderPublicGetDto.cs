using Nqey.Domain.Common;
using Nqey.Domain;
using Nqey.Api.Dtos.ProfileImageDtos;
using Nqey.Api.Dtos.ReviewDto;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderPublicGetDto
    {

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ImageDto.ImageGetDto? IdentityPiece { get; set; }
        public ImageDto.ImageGetDto? SelfieImage { get; set; }

        // you probably need to put it as ProfileImageDto 
        public ProfileImageGetDto? ProfileImage { get; set; }
        public List<PortfolioImageDto>? Portfolio { get; set; }
        public string ServiceDescription { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public Location? Location { get; set; }
        public AccountStatus AccountStatus { get; set; } 
        public int ServiceId { get; set; }
        //public Service Service { get; set; }
        public List<ReviewGetDto>? Reviews { get; set; }

    }
}
