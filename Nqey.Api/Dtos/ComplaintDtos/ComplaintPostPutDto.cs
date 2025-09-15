using System.ComponentModel.DataAnnotations;

namespace Nqey.Api.Dtos.ComplaintDtos
{
    public class ComplaintPostPutDto
    {
        public int? ReportedUserId { get; set; }
        public int? ReservationId { get; set; }
        [Required]
        public string Category { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        public List<IFormFile>? Attachments { get; set; }
    }
}
