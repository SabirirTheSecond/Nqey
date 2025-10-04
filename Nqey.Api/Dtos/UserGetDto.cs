using Nqey.Api.Dtos.ComplaintDtos;
using Nqey.Api.Dtos.MessageDtos;
using Nqey.Api.Dtos.ProfileImageDtos;
using Nqey.Domain;
using Nqey.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos
{
    public class UserGetDto
    {


        public int UserId { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        //public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public string? EmailConfirmed { get; set; }
        public string? PasswordConfirmed { get; set; }
        [Required]
        public Role UserRole { get; set; }
        [Required]
        public AccountStatus AccountStatus { get; set; }

        public ProfileImageGetDto? ProfileImage { get; set; }

        public ICollection<MessageGetDto>? SentMessages { get; set; } 
        public ICollection<MessageGetDto>? ReceivedMessages { get; set; } 
        public List<ComplaintGetDto>? FiledComplaints { get; set; }
        public List<ComplaintGetDto>? ComplaintsAgainst { get; set; } 

        public UserAnalytics UserAnalytics { get; set; } = new UserAnalytics();
    }
}
