using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Infrastructure
{
    public class NotFoundException : Exception
    {
        public string Alias { get; set; }

        public NotFoundException() : base() { }

        public NotFoundException(string alias) : base()
        {
            this.Alias = alias;
        }

        public NotFoundException(string alias, string message) : base(message)
        {
            this.Alias = alias;
        }
    }
}
