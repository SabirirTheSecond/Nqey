using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.AdminDtos;
using Nqey.DAL;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AdminController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        public AdminController(DataContext dataContext, IMapper mapper) 
        { 
            _dataContext = dataContext;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAdmins()
        {
            var admins = await  _dataContext.Users.Where(u => u.UserRole == Role.Admin).ToListAsync();
            if( !admins.Any())
                return NotFound( new ApiResponse<User>(false, "List empty"));

            return Ok(new ApiResponse<List<User>>(true,"List of admins",admins));
        }

        [HttpPost]
        public async Task<IActionResult> AddAdmin(AdminPostPutDto admin)
        {
            var domainAdmin = _mapper.Map<Admin>(admin);
            domainAdmin.SetPassword(admin.Password);
            domainAdmin.UserRole = Domain.Role.Admin;
            domainAdmin.AccountStatus = AccountStatus.Active;
            _dataContext.Users.Add(domainAdmin);
            await _dataContext.SaveChangesAsync();

            var mappedAdmin = _mapper.Map<AdminGetDto>(domainAdmin);
            return Ok(new ApiResponse<AdminGetDto>(true,"Admin added successfully",mappedAdmin));
        }
    }
}
