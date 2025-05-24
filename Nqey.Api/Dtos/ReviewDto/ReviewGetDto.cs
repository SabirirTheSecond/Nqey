using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos.ReviewDto
{
    public class ReviewGetDto
    {

        public int ReviewId { get; set; }
        public int Stars { get; set; }
        public string? Feedback { get; set; }

    }
}
