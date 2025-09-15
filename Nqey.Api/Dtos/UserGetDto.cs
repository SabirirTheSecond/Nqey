using Nqey.Domain;
using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos
{
    public class UserGetDto
    {


        public int UserId { get; set; }
        
        public  string UserName { get; set; }
        
        public  string Email { get; set; }
        
        
        public string? EmailConfirmed { get; set; }
        public string? PasswordConfirmed { get; set; }
       
        public Role UserRole { get; set; }
        
        public AccountStatus AccountStatus { get; set; }
        //public int UnreadMessagesCount { get; set; }
    }
}
