using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.DAL.Repositories;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ClientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepo;
        private readonly IUserRepository _userRepo;

        public ClientController(IMapper mapper, IClientRepository clientRepo, IUserRepository userRepository)
        {
            _mapper = mapper;
            _clientRepo = clientRepo;
            _userRepo = userRepository;

        }

        [HttpGet]
        
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepo.GetClientsAsync();
            if (clients == null)
                return NotFound(new ApiResponse<Client>(false, "Clients not found"));
            var mappedClients = _mapper.Map<List<ClientGetDto>>(clients);

            return Ok(new ApiResponse<List<ClientGetDto>>(true, "List of Clients", mappedClients));
        
        }
        [HttpGet]
        [Route("id")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);
            if (client == null)
                return NotFound(new ApiResponse<Client>(false, "Client not found"));
            var mappedClient = _mapper.Map<ClientGetDto>(client);
            return Ok(new ApiResponse<ClientGetDto>(true,"Client retrieved successfully",mappedClient));

        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromBody] ClientPostPutDto clientPostPut)
        {
            var domainClient = _mapper.Map<Client>(clientPostPut);
            var userPostPut = _mapper.Map<UserPostPutDto>(clientPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);
            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Client;
            domainUser.AccountStatus = AccountStatus.Active;

            domainClient.SetPassword(clientPostPut.Password);

            await _clientRepo.AddClientAsync(domainClient);
            await _userRepo.AddUserAsync(domainUser);
            var mappedClient = _mapper.Map<ClientGetDto>(domainClient);
            return Ok(new ApiResponse<ClientGetDto>(true, "Client Added Successfully ", mappedClient));
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientPostPutDto clientPostPut)
        {
            var oldClient = await _clientRepo.GetClientByIdAsync(id);
            if (oldClient == null)
                return NotFound(new ApiResponse<Client>(false, "Client not found"));
            _mapper.Map(clientPostPut, oldClient);
            await _clientRepo.UpdateClientAsync(oldClient);

            var mappedClient = _mapper.Map<ClientGetDto>(oldClient);
            return Ok(new ApiResponse<ClientGetDto>(true,"Client Updated Successfully", mappedClient));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var toDelete = await _clientRepo.GetClientByIdAsync(id);
            if( toDelete == null)
                return BadRequest(new ApiResponse<Client>(false,"Client Not Found"));
            await _clientRepo.DeleteClientAsync(id);
            return Ok(new ApiResponse<Client>(true, "Client Deleted Successfully"));
        }
        
    }
}
