using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Common;


namespace Nqey.DAL.Repositories
{
    public class MessageRepository(DataContext dataContext, IUserRepository userRepo) : IMessageRepository
    {
        
        public async Task<Message> AddAsync(Message message)
        {   
           await dataContext.Messages.AddAsync(message);
           await dataContext.SaveChangesAsync();

            return message;
            
        }

        public async Task<List<(int SenderId, string SenderName, string? SenderAvatar, int UnreadCount)>>
            CountUnreadMessagesAsync(int receiverId)
        {
            var grouped= await dataContext.Messages
                .AsNoTracking()
                .Where(m => m.RecieverId == receiverId && !m.IsRead)
                .Include(m => m.Sender)
                    .ThenInclude(s => s.ProfileImage)
                .GroupBy(m => new
                {
                    m.Sender.UserId,
                    m.Sender.UserName,
                    Avatar = m.Sender.ProfileImage.ImagePath
                })

                .Select(g => new
                {
                    SenderId = g.Key.UserId,
                    SenderName = g.Key.UserName,
                    SenderAvatar = g.Key.Avatar,
                    UnreadCount = g.Count()


                })

                .ToListAsync();

            return grouped.Select(g=> (g.SenderId, g.SenderName, g.SenderAvatar, g.UnreadCount)).ToList();          

        }

        public async Task<List<Message>>GetConversationAsync(int userId, int otherUserId, DateTime? sinceUtc= null)
        {
            var me = await userRepo.GetByIdAsync(userId);
            var otherUser = await userRepo.GetByIdAsync(otherUserId);
            if (otherUser == null)
            {
                return null;
            }

            var effectiveSince = sinceUtc ?? DateTime.MinValue;

            var msgs = await dataContext.Messages
                .Where(m =>
                        ((m.SenderId == userId && m.RecieverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.RecieverId == userId))
                        && m.TimeStamp >= effectiveSince
                        )
                .Include(m=>m.Sender)
                    .ThenInclude(s=>s.ProfileImage)
                .Include(m=>m.Receiver)
                    .ThenInclude(s=>s.ProfileImage)
                .OrderBy(m => m.TimeStamp)
                .ToListAsync()
                ;
            var unreadMsgs = msgs.Where(m => m.RecieverId == userId && !m.IsRead).ToList();
           
            if (unreadMsgs.Count >0)
            {
                foreach (var msg in unreadMsgs)
                    msg.IsRead = true;

                await dataContext.SaveChangesAsync();
            }
            

                return msgs ;          
            
        }

        public async Task<List<(User user, int UnreadCount)>> GetMessagedUsers(int myUserId)
        {
            var userIds = await dataContext.Messages
                 .Where(m => m.SenderId == myUserId || m.RecieverId == myUserId)
                 .Select(m => m.SenderId == myUserId ? m.RecieverId : m.SenderId)
                 .Distinct()
                 .ToListAsync();

            // Fetch all users in one go
            var users = await dataContext.Users
                .Where(u => userIds.Contains(u.UserId))
                .OrderByDescending(u=> u.UserId)
                .Include(u=>u.ProfileImage)
                .ToListAsync();
            var unreadCounts = await dataContext.Messages
                .Where(m => m.RecieverId == myUserId && !m.IsRead && userIds.Contains(m.SenderId))
                .GroupBy(m => m.SenderId)
                .Select(g => new { SenderId = g.Key, Count = g.Count() })
                .ToListAsync();

            var result = users.Select(u =>
            (
                User: u,
                UnreadCount: unreadCounts.FirstOrDefault(c => c.SenderId == u.UserId)?.Count ?? 0
            )).ToList();

            return result;
        }

        public async Task<int> MarkMessageUpAsReadAsync(int messageId, int myId)
        {
            var msg =await dataContext.Messages
                .Where(m => m.RecieverId == myId  && m.MessageId== messageId &&!m.IsRead)
                .FirstOrDefaultAsync();

            var updatedMsg = msg.IsRead = true;

            return await dataContext.SaveChangesAsync();
        }
    }
}
