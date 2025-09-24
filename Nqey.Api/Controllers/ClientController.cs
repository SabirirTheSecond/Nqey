using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.DAL.Repositories;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Nqey.Api.Dtos.ClientDtos;
using System.Security.Claims;
using Nqey.Domain.Abstractions.Services;
namespace Nqey.Api.Controllers
{

    [ApiController]
    [Route("api/[Controller]")]
    public class ClientController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClientRepository _clientRepo;
        
        private readonly IImageService _imageService;
        public ClientController(IMapper mapper, IClientRepository clientRepo
            ,  IImageService imageService)
        {

            _mapper = mapper;
            _clientRepo = clientRepo;
            
            _imageService = imageService;
        }


        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> GetClients()
        {
            var clients = await _clientRepo.GetClientsAsync();
            if (clients == null)
                return NotFound(new ApiResponse<Client>(false, "Clients not found"));
            var mappedClients = _mapper.Map<List<ClientPublicGetDto>>(clients);

            return Ok(new ApiResponse<List<ClientPublicGetDto>>(true, "List of Clients", mappedClients));
        
        }
        //[Authorize(Roles = "Admin,Provider")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetClientById(int id)
        {
            var client = await _clientRepo.GetClientByIdAsync(id);
            if (client == null)
                return NotFound(new ApiResponse<Client>(false, "Client not found"));
            
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if(User.Identity.IsAuthenticated == true && role == Role.Admin.ToString())
            {
                var mappedClient = _mapper.Map<ClientAdminGetDto>(client);
                return Ok(new ApiResponse<ClientAdminGetDto>(true, "Admin view of the client: ", mappedClient));
            }

            else
            {
                var mappedClient = _mapper.Map<ClientPublicGetDto>(client);
                return Ok(new ApiResponse<ClientPublicGetDto>(true, "Client retrieved successfully", mappedClient));
            }
                

        }

        [HttpPost]
        public async Task<IActionResult> AddClient([FromForm] ClientPostPutDto clientPostPut)
        {
            var domainClient = _mapper.Map<Client>(clientPostPut);
             domainClient.SetPassword(clientPostPut.Password);

            await _clientRepo.AddClientAsync(domainClient);
           
            if (clientPostPut.ProfileImage != null)
            {


                domainClient.ProfileImage = await _imageService.UploadImageSafe(
                    clientPostPut.ProfileImage,domainClient.UserId
                    );
                await _clientRepo.UpdateClientAsync(domainClient);
                
            }

            var mappedClient = _mapper.Map<ClientPublicGetDto>(domainClient);
            return Ok(new ApiResponse<ClientPublicGetDto>(true, "Client Added Successfully ", mappedClient));
        }

        [Authorize(Roles ="Client")]
        
        [HttpPatch]
        [Route("edit")]
        public async Task<IActionResult> UpdateClient([FromForm] ClientPatchDto clientPatchDto)
        {
            var userIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var userId)) {
                return NotFound(new ApiResponse<ClientPublicGetDto>(false,"Could Not Determine User Identity, Please Log Again"));
            }
            var oldClient = await _clientRepo.GetClientByIdAsync(userId);
            if (oldClient == null)
                return NotFound(new ApiResponse<Client>(false, "Client not found"));
           
            _mapper.Map(clientPatchDto, oldClient);

            if (clientPatchDto.ProfileImage != null)
            {


                oldClient.ProfileImage = await _imageService.UploadImageSafe(
                    clientPatchDto.ProfileImage, oldClient.UserId
                    );
            }
                await _clientRepo.UpdateClientAsync(oldClient);

            var mappedClient = _mapper.Map<ClientPublicGetDto>(oldClient);

            return Ok(new ApiResponse<ClientPublicGetDto>(true,"Client Updated Successfully", mappedClient));
        }

        [Authorize(Roles = "Admin")]
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
