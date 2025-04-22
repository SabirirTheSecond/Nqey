using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public enum Role { Client, Provider, Admin }
    

    public class User
    {

        public int UserId { get; set; }
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public  string PasswordHash { get;  set; }
        public string? EmailConfirmed { get; set; }
        public string? PasswordConfirmed { get; set; }
        [Required]
        public Role UserRole { get; set; }
        [Required]
        public AccountStatus AccountStatus { get; set; }
        public ProfileImage? ProfilePicture { get; set; }
        


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
