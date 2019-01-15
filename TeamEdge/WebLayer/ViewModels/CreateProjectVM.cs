using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.Models
{
    public class CreateProjectVM
    {
        [Required]
        [RegularExpression("^[a-zA-Z_1-9]{3,20}$")]
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Logo { get; set; }
    }
}
