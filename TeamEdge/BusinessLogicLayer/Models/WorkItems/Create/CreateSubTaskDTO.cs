using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class CreateSubTaskDTO : CreateWorkItemDTO
    {
        public int? AssignedToId { get; set; }
    }
}
