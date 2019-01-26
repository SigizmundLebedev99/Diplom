using System.Collections;
using System.Collections.Generic;

namespace TeamEdge.DAL.Mongo.Models
{
    public class WorkItemChanged : HistoryRecord
    {
        public int Number { get; set; }
        public string Code { get; set; }
        public ICollection<IPropertyChanged> Changes { get; set; }
    }
}
