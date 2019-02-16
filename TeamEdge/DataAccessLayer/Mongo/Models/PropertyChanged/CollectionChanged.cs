using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.DAL.Mongo.Models
{
    public struct CollectionChanged : IPropertyChanged
    {
        public IEnumerable<object> Added { get; set; }
        public IEnumerable<object> Deleted { get; set; }
        public string PropertyName { get; set; }
    }
}
