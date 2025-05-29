using Nqey.Domain;

namespace Nqey.Api.Dtos
{
    public class LoginDto
    {
        
        public string Username { get; set; }
        public string Password { get; set; }
        public string AppType { get; set; } // values: "Admin", "Client", or "Provider"

    }
}
