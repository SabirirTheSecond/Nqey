using Nqey.Domain;

namespace Nqey.Api.Dtos.ClientDtos
{
    public class ClientPostPutDto
    {

        public string Username { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public IFormFile? ProfilePicture { get; set; }

    }
}
