using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class GantDiagramDTO
    {
        public IEnumerable<GantChainDTO> Elements { get; set; }
    }
}
