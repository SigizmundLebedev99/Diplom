using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectsForUserDTO> GetProjectsForUserAsync(int userId);
        Task<ProjectDTO> CreateProject(int creatorId, CreateProjectDTO model);
    }
}
