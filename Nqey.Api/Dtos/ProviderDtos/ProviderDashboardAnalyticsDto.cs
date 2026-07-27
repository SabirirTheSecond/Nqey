using Nqey.Domain.Common;
using Nqey.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nqey.Api.Dtos.ProviderDtos
{
    public class ProviderDashboardAnalyticsDto
    {
        public ProviderAnalytics ProviderAnalytics { get; set; } 
        
        public double AverageRating { get; set; } 
        
        
        public double CompletionRate { get; set; }
        
        
        public double CancellationRate { get; set; }
        
        public int PendingOrders { get; set; }
        
        public int ServicesOffered { get; set; }
        
       
        public int CompletedThisMonth {  get; set; }
        

        
        public DateTime NextAppointement {  get; set; }
       
    }
}
