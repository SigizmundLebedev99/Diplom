using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class FileLightDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isPicture { get; set; }
        public string ImageBase64 { get; set; }
    }
}
