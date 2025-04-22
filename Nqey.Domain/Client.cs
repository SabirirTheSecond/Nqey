using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public enum AccountStatus { Active, Blocked };
    public class Client
    {
        public int ClientId { get; set; }
        public string UserName { get; set; }
        public ProfileImage? ProfilePicture { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get;  set; }
        public string PhoneNumber { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Active;
        public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
        public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();


        public void SetPassword(string password)
        {
            var hasher = new PasswordHasher<Client>();
            PasswordHash = hasher.HashPassword(this, password);

        }

        public bool VerifyPassword(string password)
        {
            var hasher = new PasswordHasher<Client>();
            return hasher.VerifyHashedPassword(this, PasswordHash, password) == PasswordVerificationResult.Success;
        }

    }
}
