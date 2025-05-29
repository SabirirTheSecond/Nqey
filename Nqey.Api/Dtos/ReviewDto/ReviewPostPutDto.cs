using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos.ReviewDto
{
    public class ReviewPostPutDto
    {
        
        public int Stars { get; set; }
        public string? Feedback { get; set; }
        public int ClientId { get; set; }
        public int ProviderId { get; set; }
    }
}
