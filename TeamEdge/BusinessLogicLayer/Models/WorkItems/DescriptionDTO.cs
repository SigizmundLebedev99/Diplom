using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class DescriptionDTO
    {
        public string DescriptionText { get; set; }
        public DateTime DateOfCreation { get; set; }
        public UserLightDTO CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public UserLightDTO LastUpdateBy { get; set; }
        public int FilesCount { get; set; }
    }
}
