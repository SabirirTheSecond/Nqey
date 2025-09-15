using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos.ReviewDto
{
    public class ReviewPostPutDto
    {
        public int ReservationId { get; set; }
        public int Stars { get; set; }
        public string? Feedback { get; set; } 
        //public int ProviderUserId { get; set; }
    }
}
