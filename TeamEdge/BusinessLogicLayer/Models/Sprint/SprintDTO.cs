﻿using System;

namespace TeamEdge.Models
{
    public class SprintDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
