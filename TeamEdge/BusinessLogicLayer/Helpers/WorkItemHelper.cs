using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using TeamEdge.DAL.Models;
using TeamEdge.Models;
using static System.Linq.Expressions.Expression;

namespace TeamEdge.BusinessLogicLayer
{
    static class WorkItemHelper
    {
        public static Expression<Func<T, bool>> GetFilter<T>(GetItemsDTO model) where T : IBaseWorkItem
        {
            var par = Parameter(typeof(T));

            var filter = Equal(
                Property(
                    Property(par, nameof(BaseWorkItem.Description))
                    , nameof(WorkItemDescription.ProjectId)
                    ),
                Constant(model.ProjectId)
                );

            if (model.ParentId != null)
                filter = AndAlso(filter, Equal(
                     Property(
                        Property(par, nameof(IBaseWorkItemWithParent<_Task>.ParentId))
                        , nameof(Nullable<int>.Value)
                    ),
                    Constant(model.ParentId.Value)));
            else if (model.HasNoParent)
                filter = AndAlso(filter, Equal(
                    Property(par, nameof(IBaseWorkItemWithParent<_Task>.ParentId)),
                    Constant(null)));

            return Lambda<Func<T, bool>>(filter, par);
        }

        public static Expression<Func<IBaseWorkItem, ItemDTO>> ItemDTOSelector = item => new ItemDTO
        {
            Code = item.Code,
            DescriptionId = item.DescriptionId,
            Name = item.Name,
            Number = item.Number
        };


        public static void RestoreDescriptionData(WorkItemDescription previous, WorkItemDescription nextdesc)
        {
            nextdesc.Id = previous.Id;
            nextdesc.DateOfCreation = previous.DateOfCreation;
            nextdesc.LastUpdaterId = nextdesc.CreatorId;
            nextdesc.LastUpdate = DateTime.Now;
            nextdesc.CreatorId = previous.CreatorId;
        }
    }

    class WorkItemComparer<T> : IEqualityComparer<T> where T : IBaseWorkItem
    {
        public bool Equals(T x, T y)
        {
            return x.DescriptionId == y.DescriptionId;
        }

        public int GetHashCode(T obj)
        {
            return obj.DescriptionId.GetHashCode();
        }
    }
}
