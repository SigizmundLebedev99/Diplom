using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.DAL.Models
{
    public class User : IdentityUser<int>
    {
        [StringLength(64, MinimumLength =3)]
        public string Firstname { get; set; }
        [StringLength(64, MinimumLength = 3)]
        public string Lastname { get; set; }
        [StringLength(64, MinimumLength =3)]
        public string Patrinymic { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<Subscribe> Subscribes { get; set; }
        public string Avatar { get; internal set; }

        public string FullName
        {
            get
            {
                return $"{Lastname} {Firstname}";
            }
        }
    }
}
