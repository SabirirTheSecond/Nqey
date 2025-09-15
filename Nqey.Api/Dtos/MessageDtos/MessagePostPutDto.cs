using Nqey.Domain;

namespace Nqey.Api.Dtos.MessageDtos
{
    public class MessagePostPutDto
    {
        
        //public int SenderId { get; set; }
        
        public int RecieverId { get; set; }
        
        public string Content { get; set; }
        
    }
}
