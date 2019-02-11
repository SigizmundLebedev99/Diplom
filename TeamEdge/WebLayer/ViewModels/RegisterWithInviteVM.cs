using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class RegisterWithInviteVM
    {
        public int InviteId { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Firstname { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        public string UserName { get; set; }
        public string Patronymic { get; set; }
    }
}
