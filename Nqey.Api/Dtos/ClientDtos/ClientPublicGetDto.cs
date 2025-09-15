using Microsoft.AspNetCore.Authorization;
using Nqey.Api.Dtos.ProfileImageDtos;
using Nqey.Domain;
using Nqey.Domain.Common;

namespace Nqey.Api.Dtos.ClientDtos
{
    public class ClientPublicGetDto
    {
        public int UserId { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public ProfileImageGetDto? ProfileImage { get; set; }
        public LocationDto Location { get; set; }




    }
}
