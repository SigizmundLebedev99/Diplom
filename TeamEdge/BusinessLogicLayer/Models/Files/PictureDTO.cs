using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.Models
{
    public class PictureDTO : FileDTO
    {
        public string ImageBase64 { get; set; }
    }
}
