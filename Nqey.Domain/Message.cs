using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public class Message
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; } = default!;
        public int RecieverId { get; set; }
        public User Receiver { get; set; } = default!;
        public string Content { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public bool IsRead{ get; set; } = false;


    }
}
