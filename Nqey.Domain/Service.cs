using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public class Service
    {
        public int ServiceId { get; set; } 
        public string Name { get; set; }
        public List<Provider>? Providers { get; set; }
        
    }
}
