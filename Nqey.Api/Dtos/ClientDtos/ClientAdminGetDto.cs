using Nqey.Domain.Common;
using Nqey.Domain;

namespace Nqey.Api.Dtos.ClientDtos
{
    public class ClientAdminGetDto
    {

        public int ClientId { get; set; }
        public string Username { get; set; }

        public string Email { get; set; }

        public ProfileImage ProfileImage { get; set; }
        public AccountStatus Status { get; set; } = AccountStatus.Active;

        public ICollection<Message> SentMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
    }
}
