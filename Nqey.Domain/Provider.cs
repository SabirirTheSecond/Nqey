using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public ProviderAnalytics ProviderAnalytics { get; set; } = new();

        [NotMapped]
        public double AverageRating
        {
            
            get
            {
                if (Reviews == null || Reviews.Count == 0)
                    return 0;
                var average = Reviews.Average(r => (double)r.Stars);
                var toDisplayAverage = average.ToString("F1");
                return Convert.ToDouble(toDisplayAverage);
            }

        }
        [NotMapped]
        public double CompletionRate
        {
            get
            {
                double completionRate = 0;
                if (Reservations == null || Reservations.Count == 0) return completionRate;
                
                var completedReservations = Reservations
                    .Where(r => r.Status == ReservationStatus.Completed) 
                    .Count();
                var totalReservations = Reservations.Count;
                completionRate = (double)completedReservations / totalReservations * 100;
                var toDisplay = completionRate.ToString("F2");
                return Convert.ToDouble(toDisplay);
            }
        }
        [NotMapped]
        public double CancellationRate
        {
            get
            {
                double cancellationRate = 0;
                if (Reservations == null || Reservations.Count == 0) return cancellationRate;
                var cancelledReservations = Reservations
                    .Where(r => r.Status == ReservationStatus.Cancelled)
                    .Count();
                var totalReservations = Reservations.Count;
                cancellationRate = (double)cancelledReservations / totalReservations * 100;
                var toDisplay = cancellationRate.ToString("F2");

                return Convert.ToDouble(toDisplay);
                
            }
        }
        [NotMapped]
        public int PendingOrders
        {
            get
            {
                var pendingOrders = 0;
                if (Reservations == null || Reservations.Count == 0) return pendingOrders;
                 pendingOrders = Reservations
                    .Where(r => r.Status == ReservationStatus.Pending)
                     .Count();

                return pendingOrders;
                
            }
        }
        [NotMapped]
        public int ServicesOffered
        {
            get
            {
                if (SubServices == null || SubServices.Count == 0) return 0;
                return SubServices.Count ;
            }
        }

     
        [NotMapped]
        public int CompletedThisMonth
        {
            get
            {
                var CompletedReservationsLastMonth = 0;
                if (Reservations == null || Reservations.Count == 0)
                {
                    return CompletedReservationsLastMonth;
                }

                var currentDate = DateTime.UtcNow;
                var monthAgo = currentDate.AddDays(-30);

                 CompletedReservationsLastMonth = Reservations
                    .Where(r => r.Status == ReservationStatus.Completed
                    && r.Events
                            .Any(ev => ev.ReservationEventType == ReservationEventType.Completed
                                && (ev.CreatedAt <= currentDate)
                                && (ev.CreatedAt >= monthAgo)
                     )).Count();
                   
                 
                return CompletedReservationsLastMonth;
            }
        }

        [NotMapped]
        public DateTime NextAppointement
        {
            get
            {
                if(Reservations == null || Reservations.Count ==0) return DateTime.MaxValue;
                var currentDate = DateTime.UtcNow;

                var pendingReservations = Reservations
                    .Where(r => r.Status == ReservationStatus.Pending &&
                            r.StartDate >= currentDate)
                    .OrderBy(r => (r.StartDate))
                    .Select(r => r.StartDate)
                    .FirstOrDefault()
                    ;

                return pendingReservations;
            }
        }
        public Provider()
        {

            UserRole = Role.Provider;
            AccountStatus = AccountStatus.Processing;

        }
        

    }
}
