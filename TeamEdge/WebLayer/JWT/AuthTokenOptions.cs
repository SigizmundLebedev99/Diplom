using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamEdge.JWT
{
    public static class AuthTokenOptions
    {
        public const string ISSUER = "TeamEdgeServer"; // issuer
        public const string AUDIENCE = "http://localhost:50315/"; // audience
        public const string KEY = "12DimasDiplom12_12";
        public const int LIFETIME = 60;
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
