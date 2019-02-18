using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class SummaryChainDTO : GauntChainDTO
    {
        public List<GauntChainDTO> Children { get; set; }
    }
}
