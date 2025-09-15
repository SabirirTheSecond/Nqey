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
        IMapper mapper, IUserRepository userRepo, IImageService imageService) : Controller
    {

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SendComplaint([FromForm] ComplaintPostPutDto complaintPostPut)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return StatusCode(StatusCodes.Status403Forbidden,
                    new ApiResponse<Complaint>(false, "Problem Occured with your authentication, Please Login"));
            }
            var reportedUser = await userRepo.GetByIdAsync(complaintPostPut.ReportedUserId!.Value);

            if (reportedUser == null || reportedUser.AccountStatus == AccountStatus.Blocked)
            {
                return NotFound(new ApiResponse<Complaint>(false, "User Not Found, Either Don't Exist Or Suspended"));
            }

            var domainComplaint = mapper.Map<Complaint>(complaintPostPut);
            domainComplaint.ReporterId = userId;
            

            if (complaintPostPut.Attachments != null && complaintPostPut.Attachments.Any())
            {
                domainComplaint.Attachments = await imageService.UploadAttachmentImages(
                    complaintPostPut.Attachments, domainComplaint.ComplaintId);
            }
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
            var complaints = await complaintRepository.GetComplaintsByUserIdAsync(userId);

            var mappedComplaints = mapper.Map<List<ComplaintGetDto>>(complaints);
            return Ok(new ApiResponse<List<ComplaintGetDto>>(true,$"List Of Complaints Filed By {user.UserName}",
                mappedComplaints));
        }

        // --- Implement ComplaintsAgainstUser later--------
        //------

        [HttpGet("complaintId")]

        public async Task<IActionResult> GetComplaintById(int complaintId)
        {
            var complaint = await complaintRepository.GetComplaintByIdAsync(complaintId);
            if (complaint == null)
                return NotFound();
            var mappedComplaint = mapper.Map<ComplaintGetDto>(complaint);

            return Ok(new ApiResponse<ComplaintGetDto>(true, $"Complaint Id°= {mappedComplaint.ComplaintId}",
                mappedComplaint));
        }



    }
}
