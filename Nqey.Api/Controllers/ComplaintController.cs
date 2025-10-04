using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos.ComplaintDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ComplaintController(IComplaintRepository complaintRepository,
        IMapper mapper, IUserRepository userRepo, IImageService imageService,
        IProviderRepository providerRepository, IClientRepository clientRepository) : Controller
    {

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllComplaints()
        {
           var complaints= await complaintRepository.GetAllComplaintsAsync();
            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);

            return Ok(new ApiResponse<List<ComplaintGetDto>>(true, "List Of All Complaints", mappedComplaints));
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("unresolved")]
        public async Task<IActionResult> GetAllUnresolvedComplaints()
        {
            var complaints = await complaintRepository.GetAllUnresolvedComplaintsAsync();
            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);

            return Ok(new ApiResponse<List<ComplaintGetDto>>(true, "List Of All Unresolved Complaints", mappedComplaints));
        }
        


        [Authorize]
        //[Authorize(Policy ="IsActiveAccount")]
        [HttpPost]
        public async Task<IActionResult> SendComplaint([FromForm] ComplaintPostPutDto complaintPostPut)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse<Complaint>(false, "Problem Occured with your authentication, Please Login"));
            }
            if (complaintPostPut.ReportedUserId == null || complaintPostPut.ReportedUserId == 0)
                complaintPostPut.ReportedUserId = 1;
            var reporter = await userRepo.GetByIdAsync(userId);
            var reportedUser = await userRepo.GetByIdAsync(complaintPostPut.ReportedUserId!.Value);

            if (reportedUser == null || reportedUser.AccountStatus == AccountStatus.Blocked)
            {
                return NotFound(new ApiResponse<Complaint>(false, "User Not Found, Either Not Existing Or Suspended"));
            }

            var domainComplaint = mapper.Map<Complaint>(complaintPostPut);
            domainComplaint.ReporterId = userId;
            

            if (complaintPostPut.Attachments != null && complaintPostPut.Attachments.Any())
            {
                domainComplaint.Attachments = await imageService.UploadAttachmentImages(
                    complaintPostPut.Attachments, domainComplaint.ComplaintId);
            }
            // Updating the statistical variables for each user accordingly
            reporter.UserAnalytics.FiledComplaintsCount++;
            reportedUser.UserAnalytics.ComplaintsAgainstCount++;

            await complaintRepository.AddComplaintAsync(domainComplaint);
            var mappedComplaint = mapper.Map<ComplaintGetDto>(domainComplaint);

            return Ok(new ApiResponse<ComplaintGetDto>(true,
                     "Complaint Submitted Successfully", mappedComplaint));
        }
        [Authorize(Roles =("Admin"))]
        [HttpGet("complaints/{userId}")]

        public async Task<IActionResult> GetComplaintsByUserId(int userId)
        {
          
            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiResponse<ComplaintGetDto>(false, "User Not Found"));
            }
            var complaints = await complaintRepository.GetComplaintsByUserIdAsync(userId);

            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);
            return Ok(new ApiResponse<List<ComplaintGetDto>>(true,$"List Of Complaints Filed By {user.UserName}",
                mappedComplaints));
        }
        [Authorize(Roles = ("Provider,Client"))]
        [HttpGet("user-complaints")]

        public async Task<IActionResult> GetComplaintsByUser()
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse<Complaint>(false, "Problem Occured with your authentication, Please Login"));
            }

            var user = await userRepo.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new ApiResponse<ComplaintGetDto>(false, "User Not Found"));
            }
            var complaints = await complaintRepository.GetComplaintsByUserIdAsync(userId);

            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);
            return Ok(new ApiResponse<List<ComplaintGetDto>>(true, $"List Of Your Submitted Complaints ",
                mappedComplaints));
        }
        [Authorize(Roles = ("Admin"))]
        [HttpGet("complaints-against/{userId}")]

        public async Task<IActionResult> GetComplaintsAgainstUserId(int userId)
        {

            var user = await userRepo.GetByIdAsync(userId);
            if( user == null)
            {
                return NotFound(new ApiResponse<ComplaintGetDto>(false, "User Not Found"));
            }
            var complaints = await complaintRepository.GetComplaintsAgainstUserIdAsync(userId);

            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);
            return Ok(new ApiResponse<List<ComplaintGetDto>>(true, $"List Of Complaints Filed Against {user.UserName}",
                mappedComplaints));
        }

        [HttpGet("{complaintId}")]

        public async Task<IActionResult> GetComplaintById(int complaintId)
        {
            var complaint = await complaintRepository.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
                return NotFound();
            var mappedComplaint = mapper.Map<ComplaintGetDto>(complaint);

            return Ok(new ApiResponse<ComplaintGetDto>(true, $"Complaint Id°= {mappedComplaint.ComplaintId}",
                mappedComplaint));
        }

        [Authorize(Roles ="Admin")]
        [HttpPatch("{complaintId}/resolve")]
        public async Task<IActionResult> ResolveComplaint(int complaintId)
        {
            var complaint = await complaintRepository.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
                return NotFound(new ApiResponse<ComplaintGetDto>(false, "Complaint Not Found"));
            await complaintRepository.ResolveComplaintAsync(complaintId);
            var mappedComplaint = mapper.Map<ComplaintGetDto>(complaint);
            return Ok(new ApiResponse<ComplaintGetDto>(true, "Complaint Resolved",mappedComplaint));
        }
        [Authorize(Roles = "Admin")]
        [HttpPatch("{complaintId}/dismiss")]
        public async Task<IActionResult> DismissComplaint(int complaintId)
        {
            var complaint = await complaintRepository.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
                return NotFound(new ApiResponse<ComplaintGetDto>(false, "Complaint Not Found"));
            await complaintRepository.DismissComplaintAsync(complaintId);
            var mappedComplaint = mapper.Map<ComplaintGetDto>(complaint);
            return Ok(new ApiResponse<ComplaintGetDto>(true, "Complaint Dismissed", mappedComplaint));
        }


    }
}
