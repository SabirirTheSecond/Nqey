using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.DAL;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;

namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("/[Controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepo, IMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;

        }
        [HttpGet]
        public async Task<ActionResult> GetAllUsers()
        {
           var users = await _userRepo.GetUsersAsync();
            var usersGet = _mapper.Map<List<UserGetDto>>(users);
            return Ok(new ApiResponse<List<UserGetDto>>(true, "List of users ",usersGet));
        }



    }
}
