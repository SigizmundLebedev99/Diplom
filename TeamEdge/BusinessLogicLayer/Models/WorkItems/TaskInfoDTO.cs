using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class TaskInfoDTO : WorkItemDTO
    {
        public UserDTO AssignedTo { get; set; }
    }
}
