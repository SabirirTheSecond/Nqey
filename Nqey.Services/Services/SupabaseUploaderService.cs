using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nqey.Domain.Abstractions.Services;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp;

namespace Nqey.Services.Services
{
    public class SupabaseUploaderService : IImageUploaderService
    {
        private readonly string _supabaseUrl= "https://odutwxnxiynwsgaydetg.supabase.co";
        // this is the service-role key .. yep
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
            
            
            using var originalStream =  file.OpenReadStream() ;
            await using var resizedStream = await ResizeImage(originalStream, 400, 400);

            using var content = new MultipartFormDataContent();
            var filePath = $"services/{Guid.NewGuid()}_{file.FileName}";
            var streamContent = new StreamContent(resizedStream);
            
            streamContent.Headers.ContentType= new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
            content.Add(streamContent, "file",file.FileName);

            var url = $"{_supabaseUrl}/storage/v1/object/{_bucket}/{filePath}";
            var response = await _httpClient.PostAsync(url, content) ;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Upload failed: " + await response.Content.ReadAsStringAsync());
            }
            return $"{_supabaseUrl}/storage/v1/object/public/{_bucket}/{filePath}";
        }

        private async Task<Stream> ResizeImage(Stream inputStream, int maxWidth, int maxHeight)
        {
           
            inputStream.Position = 0 ;
            using var image = await Image.LoadAsync(inputStream);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(maxWidth, maxHeight)
            }));

            var outputStream = new MemoryStream();
            await image.SaveAsJpegAsync(outputStream, new JpegEncoder { Quality = 80 });
            outputStream.Position = 0 ;
            return outputStream ;
        }
    }
    
}
