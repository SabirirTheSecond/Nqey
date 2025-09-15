using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Nqey.Domain.Common;

namespace Nqey.Domain
{
    public class Complaint
    {
        public int ComplaintId { get; set; }
        public int ReporterId { get; set; }
        public User Reporter { get; set; } = default!;
        // If The issue is with a client :
        public int? ReportedUserId { get; set; }
        public User? ReportedUser { get; set; }
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
    public enum ComplaintStatus
    {
        Pending,
        InReview,
        Resolved,
        Dismissed
    }
}
