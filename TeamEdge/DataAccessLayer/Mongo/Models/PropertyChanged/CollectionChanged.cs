using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.DAL.Mongo.Models
{
    public class CollectionChanged : PropertyChanged
    {
        public IEnumerable<object> Added { get; set; }
        public IEnumerable<object> Deleted { get; set; }
    }
}
