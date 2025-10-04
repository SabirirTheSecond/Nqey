using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.DAL;
using Nqey.DAL.Repositories;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;

        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
           var users = await _userRepo.GetUsersAsync();
            var usersGet = _mapper.Map<List<UserGetDto>>(users);
            return Ok(new ApiResponse<List<UserGetDto>>(true, "List of users ",usersGet));
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        [Route("userId")]
        public async Task<ActionResult> GetUserById(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                return NotFound(new ApiResponse<User>(false,"User Not Found"));

            var userGet = _mapper.Map<UserGetDto>(user);
            return Ok(new ApiResponse<UserGetDto>(true, "User:  ", userGet));
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{userId}/activate")]
        public async Task<IActionResult> ActivateUser(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse<UserGetDto>(false, "User Not Found"));

            await _userRepo.ActivateUser(userId);
            var mappedUser = _mapper.Map<UserGetDto>(user);
            return Ok(new ApiResponse<UserGetDto>(true, "User Account Activated", mappedUser));

        }

        [Authorize(Roles = "Admin")]
        [HttpPatch]
        [Route("{userId}/block")]
        public async Task<IActionResult> BlockUser(int userId)
        {
            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) return NotFound(new ApiResponse<UserGetDto>(false, "User Not Found"));

            await _userRepo.BlockUser(userId);
            var mappedUser = _mapper.Map<UserGetDto>(user);
            return Ok(new ApiResponse<UserGetDto>(true, "User Account Blocked", mappedUser));

        }



    }
}
