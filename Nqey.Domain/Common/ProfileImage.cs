using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class ProfileImage
    {
        public int ProfileImageId { get; set; }
        public string ImagePath { get; set; }
        public int UserId { get; set; } // it could be a Client or a Provider 
        public User User { get; set; }
    }
}
