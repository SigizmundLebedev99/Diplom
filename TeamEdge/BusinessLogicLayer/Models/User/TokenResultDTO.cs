using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.Models
{
    public class TokenResultDTO
    {
        public string Access_token { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public int UserId { get; set; }

        public string Avatar { get; set; }

        public IList<string> Roles { get; set; }

        public DateTime Start { get; set; }

        public DateTime Finish { get; set; }
    }
}
