using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nqey.Api.Dtos;
using Nqey.Api.Dtos.MessageDtos;
using Nqey.Domain;
using Nqey.Domain.Abstractions.Repositories;
using Nqey.Domain.Abstractions.Services;
using Nqey.Domain.Common;
namespace Nqey.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class MessageController : Controller
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public MessageController(IMessageService messageService, IMapper mapper,
            IUserRepository userRepository) { 
            _messageService = messageService;
            _mapper = mapper;
            _userRepository = userRepository;
        }


        // -------- Send a message to a user Endpoint
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendMessage([FromBody] MessagePostPutDto messagePostPut, int receiverId)
        {
            var domainMessage = _mapper.Map<Message>(messagePostPut);
            var userIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest(new ApiResponse<Message>(false, "Could Not Resolve The User Identity"));

            }
            domainMessage.SenderId = userId;
            domainMessage.RecieverId = receiverId;

            var senderUser = await _userRepository.GetByIdAsync(userId);
            var receiverUser = await _userRepository.GetByIdAsync(receiverId);
            await _messageService.SendAsync(domainMessage);
            var mappedMessage = _mapper.Map<MessageGetDto>(domainMessage);
            return Ok(new ApiResponse<MessageGetDto>(true, $"Message Sent Successfully To {receiverUser.UserName}",
                mappedMessage));
        }

        // Retrieve The Conversation Between the authenticated User and the Other User , you can specify
        // the Time from which you want to load the messages by passing a Valid Utc Format Date Value
        // to the sinceUtc Variable
        [Authorize]
        [HttpGet]
        [Route("otherUserId")]
        public async Task<IActionResult> GetReceivedMessagesByUserId(int otherUserId, DateTime? sinceUtc)
        {
            var userIdClaim = User.FindFirstValue("userId");
            
            if(!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest(new ApiResponse<Message>(false,"Could Not Determine User Identity," +
                    " Please Try To Login Again"));
            }
            var user = await _userRepository.GetByIdAsync(userId);
            var otherUser = await _userRepository.GetByIdAsync(otherUserId);
            if(otherUser == null || otherUser.AccountStatus == AccountStatus.Blocked)
            {
                return NotFound(new ApiResponse<Message>(false,"Problem with retrieving the other user " +
                    "either it's the wrong user or his account is suspended "));
            }
            var conversation = await _messageService.GetConversationAsync(userId, otherUserId, sinceUtc);
            
            var mappedConversation = _mapper.Map<List<MessageGetDto>>(conversation);
            return Ok(new ApiResponse<List<MessageGetDto>>(true, $"{user.UserName}'s Conversations with {otherUser
                .UserName}",mappedConversation));

        }
        [Authorize]
        [HttpGet("messaged_users")]
        public async Task<IActionResult> GetAllMessagedUsers()
        {
            var myIdClaim = User.FindFirstValue("userId");
            if (!int.TryParse(myIdClaim, out var myId))
            {
                return BadRequest(new ApiResponse<Message>(false, "An error Occured While processing your Credentials" +
                    ", Please Login Again"));
            }
            var messagedUsers = await _messageService.GetMessagedUsers(myId);
            var response = messagedUsers.Select(u => new
            {
                u.user.UserId,
                u.user.UserName,
                u.user.PhoneNumber,
                u.user.ProfileImage?.ImagePath,
                u.UnreadCount
            });
            return Ok(new ApiResponse<Object>(true, "List Of Messaged Users", response));

        }
        [Authorize]
        [HttpPatch("{messageId}/read")]
        public async Task<IActionResult> MarkAsRead(int messageId)
        {
            var myIdClaim = User.FindFirstValue("userId");
            if(!int.TryParse (myIdClaim, out var myId))
            {
                return BadRequest(new ApiResponse<Message>(false, "An error Occured While processing your Credentials" +
                    ", Please Login Again"));
            }

            await _messageService.MarkMessageUpAsReadAsync(messageId, myId);
            return Ok(new ApiResponse<Message>(true, "Message Marked Up As Read"));
        }

        [Authorize]
        [HttpGet("unread-count")]

        public async Task<IActionResult> CountUnreadMessages()
        {
            var userIdClaim= User.FindFirstValue("userId");
            if (!int.TryParse(userIdClaim, out var myId))
            {
                return BadRequest(new ApiResponse<Message>(false, "An error Occured While processing your Credentials" +
                    ", Please Login Again"));
            }
            var unreadMessages= await _messageService.CountUnreadMessagesAsync(myId);
            var result = unreadMessages.Select(g => new UnreadMessageWithSenderDto
            {
                SenderId = g.SenderId,
                SenderName = g.SenderName,
                SenderAvatar = g.SenderAvatar,
                UnreadCount = g.UnreadCount
            }).ToList();

            return Ok(new ApiResponse<List<UnreadMessageWithSenderDto>>(true,
                $"You Have Unread Messages From {result.Count} Conversations",result));
        }




    }
}
