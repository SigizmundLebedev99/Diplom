using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateEpickDTO : CreateWorkItemDTO
    {
        public int[] LinkIds { get; set; }
    }
}
