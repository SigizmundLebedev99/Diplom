using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class SprintInfoDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemDTO> Stories { get; set; }
        public IEnumerable<ItemDTO> Tasks { get; set; }
    }
}
