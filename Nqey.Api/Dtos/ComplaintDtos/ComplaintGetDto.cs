using Nqey.Domain.Common;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ComplaintDtos
{
    public class ComplaintGetDto
    {
        public int ComplaintId { get; set; }
        public int ReporterId { get; set; }
        //public UserGetDto Reporter { get; set; } = default!;
        // If The issue is with a client :
        public int? ReportedUserId { get; set; }
        //public UserGetDto? ReportedUser { get; set; }
        public int? ReservationId { get; set; }
        public Reservation? Reservation { get; set; }
        // End of Client Issue Section
        public string Category { get; set; }
        public string Description { get; set; }
        public ComplaintStatus ComplaintStatus { get; set; } = ComplaintStatus.Pending;
        public List<ComplaintAttachement> Attachments { get; set; } = new List<ComplaintAttachement>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
    }
}
