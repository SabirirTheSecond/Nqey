using Nqey.Domain;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderAdminPatchDto
    {
        public int? ServiceId { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public Sex? Sex { get; set; }
        public IFormFile? IdentityPiece { get; set; }
        public IFormFile? SelfieImage { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public List<IFormFile>? Portfolio { get; set; }
        public string? ServiceDescription { get; set; }
        //public SubService SubService { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public LocationDto? Location { get; set; }
    }
}
