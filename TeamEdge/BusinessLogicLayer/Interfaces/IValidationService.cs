using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IValidationService
    {
        Task<OperationResult> ValidateFiles(FileDTO[] files);

        Task<OperationResult> ValidateBranches(string[] branches, string repositoryPath);
    }
}
