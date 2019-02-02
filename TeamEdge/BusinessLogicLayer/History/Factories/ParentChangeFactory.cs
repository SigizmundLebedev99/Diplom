using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    class ParentChangeFactory : IHistoryRecordProduser
    {

        public ParentChangeFactory(string type)
        {
            _type = type;
        }

        private string _type;

        public IPropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var p = previous as BaseWorkItem;
            var n = next as BaseWorkItem;
            if ((p == null && n == null) || p.DescriptionId == n.DescriptionId)
                return null;

            return new SimpleValueChanged
            {
                New = new ItemDTO
                {
                    Code = n.Code,
                    DescriptionId = n.DescriptionId,
                    Number = n.Number,
                    Name = n.Name
                },
                Previous = new ItemDTO
                {
                    Code = p.Code,
                    DescriptionId = p.DescriptionId,
                    Number = p.Number,
                    Name = p.Name
                },
                Type = _type
            };
        }
    }
}
