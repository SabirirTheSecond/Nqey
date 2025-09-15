using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nqey.DAL.Repositories;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;

namespace Nqey.Services.Services
{
    public class MessageService(IUserRepository userRepo, IMessageRepository messageRepo): IMessageService
    {
       

        public async Task<List<(int SenderId, string SenderName, string? SenderAvatar, int UnreadCount)>> CountUnreadMessagesAsync(int receiverId)  
            => await messageRepo.CountUnreadMessagesAsync(receiverId);
            
        

        public async Task<List<Message>> GetConversationAsync(int userId, int otherUserId, DateTime? sinceUtc = null)
        {
            var me =        await userRepo.GetByIdAsync(userId);
            var otherUser=  await userRepo.GetByIdAsync(otherUserId);
            if(otherUser== null)
            {
                return null;
            }
            var convo = await messageRepo.GetConversationAsync(userId, otherUserId,sinceUtc);
            if(convo == null)
            {
                return null;
            }
            return convo;
        }

        public async Task<List<(User user, int UnreadCount)>> GetMessagedUsers(int myUserId)
        {
           var messagedUsers= await messageRepo.GetMessagedUsers(myUserId);
            return messagedUsers;
        }

        public async Task<int> MarkMessageUpAsReadAsync(int meSenderId, int myId) =>
        
            await messageRepo.MarkMessageUpAsReadAsync(meSenderId,  myId);
            
        public async Task<Message> SendAsync(Message message)
        {
            var senderUser= await userRepo.GetByIdAsync(message.SenderId);
            var receiverUser = await userRepo.GetByIdAsync(message.RecieverId);

            if (senderUser == null || receiverUser == null ||
                     senderUser.AccountStatus == AccountStatus.Blocked ||
                     receiverUser.AccountStatus == AccountStatus.Blocked)
            {
                return null;
            }
            await messageRepo.AddAsync(message);
            return message;
            
            
        }
    }
}
