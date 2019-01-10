using System;
using TeamEdge.Models;

namespace TeamEdge.Models
{
    public class FileDTO
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public UserLightDTO CreatedBy { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
