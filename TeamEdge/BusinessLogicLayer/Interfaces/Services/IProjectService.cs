using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectsForUserDTO> GetProjectsForUserAsync(int userId);
        Task<ProjectDTO> CreateProject(CreateProjectDTO model);
        Task<ProjectDTO> UpdateProject(int id, CreateProjectDTO model);
        Task<ProjectInfoDTO> GetProjectInfo(int id, int userId);
        Task<IEnumerable<FileDTO>> GetFilesForProject(int projectId, int userId);
        Task<IEnumerable<InviteDTO>> GetInvitesToProject(int userId, int projectId);
    }
}
