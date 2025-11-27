using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.DAL;
using Nqey.Domain.Helpers;
//using Nqey.Services.;
namespace Nqey.Services.Services
{
    public class ProviderRecommender(HttpClient httpClient)
    {
         public async Task<List<int>> RecommendProvidersAsync(ProviderRecommenderRequestDto req)
        {
            var url = "https://nqey-ml.onrender.com";
            var response = await httpClient.PostAsJsonAsync($"{url}/recommend/providers"
                , req);

            //response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadFromJsonAsync<RecoResponseDto>();
            return json?.Recommended_Provider_Ids ?? new List<int>();

        }

    }

}

