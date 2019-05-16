using System;
using System.Collections.Generic;
using System.Linq;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    class FileChangeFactory : IHistoryRecordProduser
    {
        string _type;

        public FileChangeFactory(string type)
        {
            _type = type;
        }

        private static Func<WorkItemFile, FileLightDTO> selector = f => new FileLightDTO
        {
            Id = f.FileId,
            Name = f.File.FileName
        };

        public PropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var prev = previous as IEnumerable<WorkItemFile>;
            var nex = next as IEnumerable<WorkItemFile>;

            if ((next == null || nex.Count() == 0) && (previous == null || prev.Count() == 0))
                return null;

            else if (prev == null || prev.Count() == 0)
            {
                return new CollectionChanged
                {
                    PropertyName = _type,
                    Added = nex.Select(selector)
                };
            }

            else if (nex == null || nex.Count() == 0)
            {
                return new CollectionChanged
                {
                    PropertyName = _type,
                    Deleted = prev.Select(selector)
                };
            }

            else
            {
                var added = nex.Except(prev, new FileComparer()).Select(selector);
                var deleted = prev.Except(nex, new FileComparer()).Select(selector);
                if (added.Count() == 0 && deleted.Count() == 0)
                    return null;
                return new CollectionChanged
                {
                    PropertyName = _type,
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
