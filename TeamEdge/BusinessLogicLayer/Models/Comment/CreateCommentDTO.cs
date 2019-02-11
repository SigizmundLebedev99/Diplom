using System;
using System.Collections.Generic;
using System.Linq;

namespace TeamEdge.Models
{
    public class CreateCommentDTO
    {
        public string Text { get; set; }
        public int WorkItemId { get; set; }
        public UserDTO From { get; set; }
        public int[] Files { get; set; }
    }
}
