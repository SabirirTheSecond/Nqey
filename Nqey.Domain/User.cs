using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public enum Role { Client, Provider, Admin }
    public enum Sex { Male, Female}

    public abstract class User
    {

        public int UserId { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public DateTime? BirthDate { get; set; }
        public Sex? Sex { get; set; }
        public  string PasswordHash { get;  set; }
        public string PhoneNumber { get; set; }
        public string? EmailConfirmed { get; set; }
        public string? PasswordConfirmed { get; set; }
        [Required]
        public Role UserRole { get; set; }
        [Required]
        public AccountStatus AccountStatus { get; set; }
  
        public ProfileImage? ProfileImage { get; set; }

        public ICollection<Message>? SentMessages { get; set; } = new List<Message>();
        public ICollection<Message>? ReceivedMessages { get; set; } = new List<Message>();
        public List<Complaint>? FiledComplaints { get; set; } = new List<Complaint>();
        public List<Complaint>? ComplaintsAgainst { get; set; } = new List<Complaint>();

        public UserAnalytics UserAnalytics { get; set; } = new UserAnalytics();
        public void SetPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            PasswordHash = hasher.HashPassword(this, password);

        }

        public bool VerifyPassword(string password)
        {
            var hasher = new PasswordHasher<User>();
            return hasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
        }
    }
}
