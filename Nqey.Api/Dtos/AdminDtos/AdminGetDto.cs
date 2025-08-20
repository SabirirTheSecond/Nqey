namespace Nqey.Api.Dtos.AdminDtos
{
    public class AdminGetDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public LocationDto Location { get; set; }
    }
}
