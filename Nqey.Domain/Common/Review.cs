using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class Review
    {
        public int ReviewId { get; set; }
        [Range(1,5)]
        public int Stars {  get; set; }
        public string? Feedback { get; set; }

        public int ProviderId { get; set; }
        public Provider Provider { get; set; }

    }
}
