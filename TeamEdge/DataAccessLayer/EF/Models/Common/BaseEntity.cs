using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamEdge.DAL.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public User Creator { get; set; }
    }
}
