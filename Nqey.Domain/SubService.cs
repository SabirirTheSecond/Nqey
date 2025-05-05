using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain
{
    public class SubService
    {
// title,description, price, unity
        public int SubServiceId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Unity {  get; set; }
        
        public int ProviderId { get; set; }
        public Provider Provider { get; set; }
    }
}
