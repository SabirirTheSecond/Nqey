namespace Nqey.Api.Dtos.MessageDtos
{
    public class UnreadMessageWithSenderDto
    {
        public int SenderId { get; set; }
        public string SenderName { get; set; }
        public string? SenderAvatar { get; set; }
        public int UnreadCount { get; set; }
    }
}
