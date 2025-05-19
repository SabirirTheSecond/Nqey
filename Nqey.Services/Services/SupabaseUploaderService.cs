using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nqey.Domain.Abstractions.Services;

namespace Nqey.Services.Services
{
    public class SupabaseUploaderService : IImageUploaderService
    {
        private readonly string _supabaseUrl= "https://odutwxnxiynwsgaydetg.supabase.co";
        // service-role key .. yep
        private readonly string _supabaseKey = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im9kdXR3eG54aXlud3NnYXlkZXRnIiwicm9sZSI6InNlcnZpY2Vfcm9sZSIsImlhdCI6MTc0NzY3NTY5OCwiZXhwIjoyMDYzMjUxNjk4fQ.pdGOka67qj2aws23WdxFGzobZF2lcZhSvFgq33wQanw";
        private readonly string _bucket = "service-image";
        private readonly HttpClient _httpClient;

        public SupabaseUploaderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _supabaseKey);
        }

        public async Task<string> UploadImageToSupabase(IFormFile file)
        {
            var filePath = $"services/{Guid.NewGuid()}_{file.FileName}";
            using var content = new MultipartFormDataContent();
            using var stream = file.OpenReadStream() ;
            content.Add(new StreamContent(stream), "file",file.FileName);

            var url = $"{_supabaseUrl}/storage/v1/object/{_bucket}/{filePath}";
            var response = await _httpClient.PostAsync(url, content) ;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Upload failed: " + await response.Content.ReadAsStringAsync());
            }
            return $"{_supabaseUrl}/storage/v1/object/public/{_bucket}/{filePath}";
        }
    }
}
