using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class GantChainDTO
    {
        public string Code { get; set; }

        public WorkItemStatus Status { get; set; }
        public int DescriptionId { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public int? PreviousId { get; set; }
        public GantChainDTO Next { get; set; }

        public int? ParentId { get; set; }
    }
}
