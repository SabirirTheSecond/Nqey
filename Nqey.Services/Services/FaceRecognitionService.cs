using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Helpers;

namespace Nqey.Services.Services
{
    public class FaceRecognitionService: IFaceRecognitionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _verifyUrl;
        public FaceRecognitionService(IHttpClientFactory httpClientFactory,
            IOptions<MLApiOptions> mlApiOptions) {
            
            _httpClientFactory = httpClientFactory;
            _verifyUrl= mlApiOptions.Value.VerifyUrl;

        }

        public async Task<bool> VerifyFacesAsync(string idImageUrl, string selfieImageUrl)
        {
            using var client= _httpClientFactory.CreateClient();
            var payload = new
            {
                img1_path = idImageUrl,
                img2_path = selfieImageUrl
            };

            var response = await client.PostAsJsonAsync(_verifyUrl, payload);
            if (!response.IsSuccessStatusCode) return false;
            var result = await response.Content.ReadFromJsonAsync<DeepFaceResult>();
            return result?.verified ?? false;

            
        }
    }
}
