using System.Collections.Generic;
using System.Linq;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class FileChangeFactory : IHistoryRecordProduser
    {
        string _type;

        public FileChangeFactory(string type)
        {
            _type = type;
        }

        public IPropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var prev = previous as IEnumerable<WorkItemFile>;
            var nex = next as IEnumerable<WorkItemFile>;

            if ((next == null || nex.Count() == 0) && (previous == null || prev.Count() == 0))
                return null;

            else if (prev == null || prev.Count() == 0)
            {
                return new CollectionChanged
                {
                    Type = _type,
                    Added = nex.Select(e => new { Id = e.FileId, Name = e.File.FileName })
                };
            }

            else if (nex == null || nex.Count() == 0)
            {
                return new CollectionChanged
                {
                    Type = _type,
                    Deleted = prev.Select(e => new { Id = e.FileId, Name = e.File.FileName })
                };
            }

            else
            {
                var added = nex.Except(prev, new FileComparer()).Select(e => new { Id = e.FileId, Name = e.File.FileName });
                var deleted = prev.Except(nex, new FileComparer()).Select(e => new { Id = e.FileId, Name = e.File.FileName });
                if (added.Count() == 0 && deleted.Count() == 0)
                    return null;
                return new CollectionChanged
                {
                    Type = _type,
                    Added = added,
                    Deleted = deleted
                };
            }
           
        }
    }

    class FileComparer : IEqualityComparer<WorkItemFile>
    {
        public bool Equals(WorkItemFile x, WorkItemFile y)
        {
            return x.FileId == y.FileId;
        }

        public int GetHashCode(WorkItemFile obj)
        {
            return obj.FileId.GetHashCode();
        }
    }
}
