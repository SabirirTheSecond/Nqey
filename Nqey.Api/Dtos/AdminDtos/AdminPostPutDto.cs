namespace Nqey.Api.Dtos.AdminDtos
{
    public class AdminPostPutDto
    {
        public string Username { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public LocationDto? Location { get; set; }
    }
}
