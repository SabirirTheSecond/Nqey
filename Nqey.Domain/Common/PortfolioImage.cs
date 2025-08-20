using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class PortfolioImage
    {
        public int PortfolioImageId { get; set; }
        public string ImagePath { get; set; }
        public int ProviderUserId { get; set; }
        public Provider Provider { get; set; }

    }
}
