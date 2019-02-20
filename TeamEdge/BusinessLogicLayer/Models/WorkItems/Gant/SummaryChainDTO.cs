using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class SummaryChainDTO : GantChainDTO
    {
        public List<GantChainDTO> Children { get; set; }
    }
}
