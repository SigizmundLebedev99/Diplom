using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class ItemWithChildrenDTO : ItemDTO
    {
        public IEnumerable<ItemWithChildrenDTO> Children { get; set; }
    }
}
