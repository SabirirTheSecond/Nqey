using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public class Provider : User
    {
       
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Image? IdentityPiece { get; set; }
        public int? IdentityId { get; set; }
        public Image? SelfieImage { get; set; }
        public int? SelfieId { get; set; }
       
        public string ServiceDescription { get; set; } 
        public  Location? Location { get; set; } 
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }      
        public List<Review>? Reviews { get; set; } = new List<Review>();
        public List<SubService>? SubServices { get; set; } = new List<SubService>();
        
        public List<PortfolioImage>? Portfolio{ get; set; } = new List<PortfolioImage>();
        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
        public int JobsDone { get; set; } = 0;
        public bool IsIdentityVerified { get; set; } = false;
        public ServiceRequest? ServiceRequest { get; set; }
        
        public Provider()
        {
            UserRole = Role.Provider;
            AccountStatus = AccountStatus.Blocked;
        }
        

    }
}
