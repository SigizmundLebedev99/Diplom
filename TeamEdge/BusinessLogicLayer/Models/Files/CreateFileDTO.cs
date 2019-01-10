using Microsoft.AspNetCore.Http;

namespace TeamEdge.Models
{
    public class CreateFileDTO
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public IFormFile File { get; set; }
    }
}
