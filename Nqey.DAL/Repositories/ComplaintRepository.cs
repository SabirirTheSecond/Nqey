using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;

namespace Nqey.DAL.Repositories
{
    public class ComplaintRepository(DataContext dataContext) : IComplaintRepository
    {
        public async Task<List<Complaint>> GetAllComplaintsAsync()
        {
            return await dataContext.Complaints.ToListAsync();
        }
        public async Task<List<Complaint>> GetAllUnresolvedComplaintsAsync()
        {
            var unresolved= await dataContext.Complaints
                .Where(c=> c.ComplaintStatus== ComplaintStatus.Pending)
                .ToListAsync();
            return unresolved;
        }
        public async Task<Complaint> AddComplaintAsync(Complaint complaint)
        {
            
            await dataContext.Complaints.AddAsync(complaint);
            await dataContext.SaveChangesAsync();
            return complaint;
        }

        public async Task<Complaint> GetComplaintByIdAsync(int complaintId)
        {
            var complaint = await dataContext.Complaints
                 .Include(c => c.Reporter)
                .Include(c => c.ReportedUser)
                .Include(c => c.Attachments)
                .FirstOrDefaultAsync(c=> c.ComplaintId == complaintId);
           
            if(complaint == null)
            {
                return null;
            }

            return complaint;
        }


        public async Task<List<Complaint>> GetComplaintsByUserIdAsync(int userId)
        {
            var complaints = await dataContext.Complaints
                .Where(c=> c.ReporterId == userId)
                 .Include(c => c.Reporter)
                 .Include(c => c.ReportedUser)
                 .Include(c => c.Attachments)
                .ToListAsync();
            return complaints;
            
        }
        public async Task<List<Complaint>> GetComplaintsAgainstUserIdAsync(int userId)
        {
            var complaints = await dataContext.Complaints
                .Where(c => c.ReportedUserId == userId)
                 .Include(c => c.Reporter)
                 .Include(c => c.ReportedUser)
                 .Include(c => c.Attachments)
                .ToListAsync();
            return complaints;

        }

        public Task<Complaint> UpdateComplaintAsync(Complaint complaint)
        {

            throw new NotImplementedException();
        }

        public async Task<Complaint> ResolveComplaintAsync(int complaintId)
        {
            var toResolve = await dataContext.Complaints.FirstOrDefaultAsync(c=>c.ComplaintId== complaintId);
            if (toResolve == null)
                return null;
            toResolve.ComplaintStatus= ComplaintStatus.Resolved;
            toResolve.ResolvedAt = DateTime.UtcNow;

            await dataContext.SaveChangesAsync();
            return toResolve;
        }

        public async Task<Complaint> DismissComplaintAsync(int complaintId)
        {
            var toDismiss = await dataContext.Complaints.FirstOrDefaultAsync(c => c.ComplaintId == complaintId);
            if (toDismiss == null)
                return null;
            toDismiss.ComplaintStatus = ComplaintStatus.Dismissed;
            toDismiss.ResolvedAt = DateTime.UtcNow;
            await dataContext.SaveChangesAsync();
            return toDismiss;
        }
    }
}
