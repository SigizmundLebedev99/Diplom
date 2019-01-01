using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class DescriptionDTO
    {
        public string Description { get; set; }
        public string DescriptionCode { get; set; }
        public DateTime DateOfCreation { get; set; }
        public UserDTO CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public UserDTO LastUpdateBy { get; set; }
    }
}
