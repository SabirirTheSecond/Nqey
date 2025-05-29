using System.ComponentModel.DataAnnotations;
using Nqey.Api.Dtos.ClientDtos;

namespace Nqey.Api.Dtos.ReviewDto
{
    public class ReviewGetDto
    {

        public int ReviewId { get; set; }
        public int Stars { get; set; }
        public string? Feedback { get; set; }
        public ClientPublicGetDto Client { get; set; }
       

    }
}
