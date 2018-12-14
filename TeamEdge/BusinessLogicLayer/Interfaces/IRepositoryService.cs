using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IRepositoryService
    {
        bool HasPermission(int userId, int repositoryId, RepositoryAccessLevel requiredLevel);
        bool HasCreatePermission(int userId);
        bool HasPermission(int userId, string repositoryName, RepositoryAccessLevel requiredLevel);
    }
}
