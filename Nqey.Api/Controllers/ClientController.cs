﻿using AutoMapper;
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
        private readonly IUserRepository _userRepo;
        private readonly IImageUploaderService _imageUploaderService;
        public ClientController(IMapper mapper, IClientRepository clientRepo
            , IUserRepository userRepository, IImageUploaderService imageUploader)
        {

            _mapper = mapper;
            _clientRepo = clientRepo;
            _userRepo = userRepository;
            _imageUploaderService = imageUploader;
         
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

            string? imagePath = null;

            if (clientPostPut.ProfileImage != null)
            {
                try
                {
                    imagePath = await _imageUploaderService.UploadImageToSupabase(clientPostPut.ProfileImage);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new ApiResponse<string>(false, "Failed to upload image to Supabase", ex.Message));
                }
                //var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "profiles");
                //Directory.CreateDirectory(uploadsFolder);

                //var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(clientPostPut.ProfileImage.FileName);
                //var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                //using (var stream = new FileStream(filePath, FileMode.Create))
                //{
                //    await clientPostPut.ProfileImage.CopyToAsync(stream);
                //}

                //imagePath = Path.Combine("images", "profiles", uniqueFileName);
            }


            var domainClient = _mapper.Map<Client>(clientPostPut);
          
            var userPostPut = _mapper.Map<UserPostPutDto>(clientPostPut);
            var domainUser = _mapper.Map<User>(userPostPut);

           

            domainUser.SetPassword(userPostPut.Password);
            domainUser.UserRole = Role.Client;
            domainUser.AccountStatus = AccountStatus.Active;
            domainUser.PhoneNumber = clientPostPut.PhoneNumber;

            domainClient.SetPassword(clientPostPut.Password);

            await _clientRepo.AddClientAsync(domainClient);
            await _userRepo.AddUserAsync(domainUser);
            if (imagePath != null)
            {
                

                domainClient.ProfileImage = new ProfileImage
                {
                    ImagePath = imagePath,
                    UserId = domainUser.UserId // If you generate ID before save, otherwise leave out and EF will link after
               
                };
                await _clientRepo.UpdateClientAsync(domainClient);
                
            }

            var mappedClient = _mapper.Map<ClientPublicGetDto>(domainClient);
            return Ok(new ApiResponse<ClientPublicGetDto>(true, "Client Added Successfully ", mappedClient));
        }

        [Authorize(Roles ="Admin,Client")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateClient(int id, ClientPostPutDto clientPostPut)
        {
            var oldClient = await _clientRepo.GetClientByIdAsync(id);
            if (oldClient == null)
                return NotFound(new ApiResponse<Client>(false, "Client not found"));
           
            _mapper.Map(clientPostPut, oldClient);
            

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
