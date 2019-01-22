using System;
using System.Diagnostics;

namespace TeamEdge.BusinessLogicLayer.Infrastructure
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : base()
        {
        }
        public UnauthorizedException(string message) : base(message)
        { 
        }
    }
}
