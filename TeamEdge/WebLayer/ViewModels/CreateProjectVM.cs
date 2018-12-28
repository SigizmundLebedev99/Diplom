using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.Models
{
    public struct CreateProjectVM
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
    }
}
