using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IComplaintRepository
    {
        Task<Complaint> AddComplaintAsync(Complaint complaint);
        Task<Complaint> GetComplaintByIdAsync(int complaintId);
        Task<List<Complaint>> GetAllUnresolvedComplaintsAsync();
        Task<List<Complaint>> GetAllComplaintsAsync();
        Task<List<Complaint>> GetComplaintsByUserIdAsync(int userId);
        Task<List<Complaint>> GetComplaintsAgainstUserIdAsync(int userId);
        Task<Complaint> UpdateComplaintAsync(Complaint complaint);
        Task<Complaint> ResolveComplaintAsync(int complaintId);
        Task<Complaint> DismissComplaintAsync(int complaintId);



        //Task<Complaint> Res
    }
}
