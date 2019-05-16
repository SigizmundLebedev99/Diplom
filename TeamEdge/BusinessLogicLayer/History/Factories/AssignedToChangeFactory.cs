using System;
using TeamEdge.DAL.Models;
using TeamEdge.DAL.Mongo.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Infrostructure
{
    public class AssignedToChangeFactory : IHistoryRecordProduser
    {
        public AssignedToChangeFactory(string type)
        {
            _type = type;
        }

        private string _type;

        public PropertyChanged CreateHistoryRecord(object previous, object next)
        {
            var prev = previous as User;
            var nex = next as User;
            if ((prev == null && nex == null))
                return null;
            if (prev?.Id == nex?.Id)
                return null;
            return new SimpleValueChanged
            {
                Previous = prev == null ? null : new UserLightDTO { Avatar = prev.Avatar, Id = prev.Id, Name = prev.FullName },
                New = next == null ? null : new UserLightDTO { Avatar = nex.Avatar, Id = nex.Id, Name = nex.FullName },
                PropertyName = _type
            };
        }
    }
}
