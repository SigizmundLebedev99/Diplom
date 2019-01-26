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

            return new CollectionChanged
            {
                Type = _type,
                Added = nex.Except(prev, new FileComparer()).Select(e => new { Id = e.FileId, Name = e.File.FileName }),
                Deleted = prev.Except(nex, new FileComparer()).Select(e => new { Id = e.FileId, Name = e.File.FileName })
            };
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
