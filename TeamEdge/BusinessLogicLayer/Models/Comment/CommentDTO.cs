using System;
using System.Collections.Generic;

namespace TeamEdge.Models
{
    public class CommentDTO
    {
        public UserLightDTO User { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Text { get; set; }
        public IEnumerable<FileLightDTO> Files { get; set; }
    }
}
