﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public struct InviteCodeDTO
    {
        public int InviteId { get; set; }
        public string FromFullName { get; set; }
        public string FromAvatar { get; set; }
        public string FromEmail { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public string Logo { get; set; }
        public DateTime DateOfCreation { get; set; }
        public string Code { get; set; }
    }
}
