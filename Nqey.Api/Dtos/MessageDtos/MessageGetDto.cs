using Nqey.Domain;

namespace Nqey.Api.Dtos.MessageDtos
{
    public class MessageGetDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public UserGetDto Sender { get; set; } = default!;
        public int RecieverId { get; set; }
        public UserGetDto Receiver { get; set; } = default!;
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
    }
}
