using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Services
{
    public interface IRecommendationService
    {
        List<Provider> GetSortedProvidersForClients(Client client, List<Provider> providers);
        List<Provider> GetSortedProvidersForAnonymous( List<Provider> providers);
    }
}
