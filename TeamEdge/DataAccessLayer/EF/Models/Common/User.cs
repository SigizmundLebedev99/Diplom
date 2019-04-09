using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamEdge.DAL.Models
{
    public class User : IdentityUser<int>
    {
        [StringLength(64, MinimumLength =3)]
        public string FirstName { get; set; }
        [StringLength(64, MinimumLength = 3)]
        public string LastName { get; set; }
        [StringLength(64, MinimumLength =3)]
        public string Patrinymic { get; set; }

        public virtual ICollection<UserProject> UserProjects { get; set; }
        public virtual ICollection<Invite> Invites { get; set; }
        public virtual ICollection<Subscribe> Subscribes { get; set; }
        public string Avatar { get; set; }

        public string FullName
        {
            get
            {
                return $"{LastName} {FirstName}";
            }
        }
    }
}
