using System;
using System.Linq.Expressions;
using TeamEdge.DAL.Models;
using TeamEdge.Models;
using static System.Linq.Expressions.Expression;

namespace TeamEdge.BusinessLogicLayer
{
    public static class WorkItemHelper
    {
        public static Expression<Func<T, bool>> GetFilter<T>(GetItemsDTO model) where T : BaseWorkItem
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
                        Property(par, nameof(BaseWorkItem<_Task, _Task>.ParentId))
                        , nameof(Nullable<int>.Value)
                    ),
                    Constant(model.ParentId.Value)));
            else if (model.HasNoParent)
                filter = AndAlso(filter, Equal(
                    Property(par, nameof(BaseWorkItem<_Task, _Task>.ParentId)),
                    Constant(null)));

            return Lambda<Func<T, bool>>(filter, par);
        }

        public static Expression<Func<BaseWorkItem, ItemDTO>> ItemDTOSelector = item => new ItemDTO
        {
            Code = item.Code,
            DescriptionId = item.DescriptionId,
            Name = item.Name,
            Number = item.Number
        };
    }
}
