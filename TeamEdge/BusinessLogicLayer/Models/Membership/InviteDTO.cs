﻿using System;

namespace TeamEdge.Models
{
    public struct InviteDTO
    {
        public int InviteId { get; set; }
        public int FromId { get; set; }
        public string FromFullName { get; set; }
        public string FromAvatar { get; set; }
        public string FromEmail { get; set; }
        public string Email { get; set; }
        public string ProjectName { get; set; }
        public string Logo { get; set; }
        public int ProjectId { get; set; }
        public DateTime DateOfCreation { get; set; }
    }
}
