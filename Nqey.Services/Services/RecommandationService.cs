using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Helpers;

namespace Nqey.Services.Services
{
    public class RecommandationService : IRecommendationService
    {
        public RecommandationService() { 
        
        }

        public List<Provider> GetSortedProviders(Client? client, List<Provider> providers)
        {
            if(client?.Location?.Position == null) {
                Console.WriteLine($"the Position is null? {client?.Location?.Position}" +
                    $"or maybe the client ? {client?.UserName}");
                return  providers;
            }
             var sortedProviders =  providers
                    .Select(p => new { Provider = p, Score = ScoreCalculator.CalculateScore(client, p) })
                    .OrderByDescending(p => p.Score)
                    .Select(p => p.Provider)
                    .ToList();
            Console.WriteLine($"We are inside the part that works {providers}");

            return sortedProviders;
        }
    }
}
