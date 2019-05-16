using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    class ChildrenChangeFactory : IHistoryRecordProduser
    {
        readonly string _type;

        private static Func<BaseWorkItem, ItemDTO> selector = e => new ItemDTO
        {
            DescriptionId = e.DescriptionId,
            Code = e.Code,
            Name = e.Name,
            Number = e.Number
        };

        public ChildrenChangeFactory(string type)
        {
            _type = type;
        }

        public PropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var prev = previous as IEnumerable<BaseWorkItem>;
            var nex = next as IEnumerable<BaseWorkItem>;

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
                var added = nex.Except(prev, new WorkItemComparer<BaseWorkItem>()).Select(selector);
                var deleted = prev.Except(nex, new WorkItemComparer<BaseWorkItem>()).Select(selector);
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
}
