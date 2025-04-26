using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nqey.Domain.Common
{
    public class Position
    {

        public int Latitude { get; set; }
        public int Longitude { get; set; }
        public int? Accuracy  { get; set; }


    }
    public class Location
    {
       
        public int LocationId { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public Position Position { get; set; }
        public string Ville { get; set; }
        public string Wilaya  { get; set; }
        

       
    }
}
