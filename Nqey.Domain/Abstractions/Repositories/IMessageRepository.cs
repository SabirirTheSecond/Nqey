using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Abstractions.Repositories
{
    public interface IMessageRepository
    {
        Task<Message> AddAsync(Message message);
        Task<List<Message>> GetConversationAsync(int userId, int otherUserId, DateTime? sinceUtc = null);
        Task<List<(int SenderId, string SenderName, string? SenderAvatar, int UnreadCount)>> CountUnreadMessagesAsync(int receiverId);
        Task<int> MarkMessageUpAsReadAsync(int messageId, int myId);
        Task<List<(User user, int UnreadCount)>> GetMessagedUsers(int myUserId);


    }
}
