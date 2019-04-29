using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.DAL.Models
{
    public interface IBaseWorkItem
    {
        int DescriptionId { get; set; }
        WorkItemDescription Description { get; set; }

        WorkItemStatus Status { get; set; }
        int Number { get; set; }
        string Name { get; set; }

        string Code { get; }
    }

    public interface IBaseWorkItemWithChild<TChild> : IBaseWorkItem where TChild : IBaseWorkItem
    {
        ICollection<TChild> Children { get; set; }
    }

    public interface IBaseWorkItemWithParent : IBaseWorkItem
    {
        int? ParentId { get; set; }
    }

    public interface IBaseWorkItemWithParent<TParent> : IBaseWorkItemWithParent where TParent : IBaseWorkItem
    {
        TParent Parent { get; set; }
    }
}
