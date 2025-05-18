using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain.Common;

namespace Nqey.Domain.Helpers
{
    public static class ScoreCalculator
    {
        public static double CalculateScore(Client client, Provider provider) 
        {
            double score = 0;
            double distanceScore = 0;
            if (client.Location != null && provider != null)
            {
                var distance = GetDistance(client.Location.Position, provider.Location.Position);
                // Normalization: inverse scale (0 if >100km 1, if the same) 
                 distanceScore = 1 - Math.Min(distance, 100) / 100.0;
            }

            double ratingScore = 0;
            if(provider.Reviews != null && provider.Reviews.Any())
            {
             var avgRating =  provider.Reviews.Average(r => r.Stars);
                ratingScore = avgRating / 5.0;
            }

            // Normalization (on 100 jobs) 
            double jobScore = Math.Min(provider.JobsDone, 100) / 100;
            // Account active
            double accountScore = provider.AccountStatus == AccountStatus.Active ? 1.0 : 0.0;
            // Portfolio size
            //double portfolioScore = provider.Portfolio != null && provider.Portfolio.Count >= 3 ? 1.0 : 0.0;
           
            // Weighted sum
            score = distanceScore * 0.35 +
                    ratingScore * 0.3 +
                    jobScore * 0.2 +
                    accountScore * 0.1 
                    
                    //+ portfolioScore * 0.05
                    ;


            return score;
        }

        private static double GetDistance(Position p1, Position p2)
        {
            const double R = 6371; // Radius of Earth in km
            var dLat = ToRadians(p2.Latitude - p1.Latitude);
            var dLon = ToRadians(p2.Longitude - p1.Longitude);
            var lat1 = ToRadians(p1.Latitude);
            var lat2 = ToRadians(p2.Latitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }
        private static double ToRadians(double angle) => angle * Math.PI / 180;
    }
}
