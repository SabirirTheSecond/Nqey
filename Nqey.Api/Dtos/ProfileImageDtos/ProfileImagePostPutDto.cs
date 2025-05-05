using Nqey.Domain;

namespace Nqey.Api.Dtos.ProfileImageDtos
{
    public class ProfileImagePostPutDto
    {
        
        public string ImagePath { get; set; }
        public int UserId { get; set; } // it could be a Client or a Provider 
        //public User User { get; set; }
    }
}
