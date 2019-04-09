using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class ItemForBacklogDTO : ItemDTO
    {
        public int? ParentId { get; set; }
    }
}
