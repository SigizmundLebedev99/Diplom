﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.Models
{
    public class RegisterUserDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

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
