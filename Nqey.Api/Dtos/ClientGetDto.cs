using Nqey.Domain;

namespace Nqey.Api.Dtos
{
    public class ClientGetDto
    {
        public int ClientId { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }
       

        public AccountStatus Status { get; set; } = AccountStatus.Active;
        
    }
}
