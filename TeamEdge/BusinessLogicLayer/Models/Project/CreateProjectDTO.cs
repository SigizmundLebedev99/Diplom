using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models;
{
    public class CreateProjectDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
