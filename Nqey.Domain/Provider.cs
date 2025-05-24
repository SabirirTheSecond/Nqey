using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public class Provider
    {
       public int ProviderId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? IdentityPiece { get; set; }
        public int? PImageId { get; set; }
        public ProfileImage? ProfileImage { get; set; }
        public string ServiceDescription { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get;  set; }
        public string PhoneNumber { get; set; }
        public  Location? Location { get; set; }
        public AccountStatus AccountStatus { get; set; } = AccountStatus.Blocked;
        public int ServiceId { get; set; }
        public Service Service { get; set; }
        //public int? ReviewId { get; set; }
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<SubService>? SubServices { get; set; } = new List<SubService>();
        public  ICollection<Message>? SentMessages { get; set; } = new List<Message>();
        public  ICollection<Message>? ReceivedMessages { get; set; } = new List<Message>();
        public List<PortfolioImage>? Portfolio{ get; set; } = new List<PortfolioImage>();
        public int JobsDone { get; set; } = 0;
        
        public void SetPassword(string password)
        {
            var hasher = new PasswordHasher<Provider>(); 
            PasswordHash = hasher.HashPassword(this, password);

        }

        public bool VerifyPassword(string password)
        {
            var hasher = new PasswordHasher<Provider>();
            return hasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
        }

    }
}
