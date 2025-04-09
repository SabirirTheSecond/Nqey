using Nqey.Domain;
using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos
{
    public class UserPostPutDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public Role UserRole { get; set; }
        [Required]
        public AccountStatus AccountStatus { get; set; }
    }
}
