﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct CreateProjectDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
    }
}
