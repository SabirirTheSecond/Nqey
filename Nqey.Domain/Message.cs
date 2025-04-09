using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int RecieverId { get; set; }

        public string Content { get; set; }
        public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
        public bool IsRead{ get; set; } = false;


    }
}
