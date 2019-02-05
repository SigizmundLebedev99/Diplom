using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IFileWorkService
    {
        Task<(byte[] bytes, string type, string name)> GetFile(int fileId, int userId);
        Task<FileDTO> CreateFile(CreateFileDTO file);
        Task<IEnumerable<FileDTO>> GetFilesForProject(int userId, int projectId);
        Task DeleteFile(int userId, int fileId);
        Task<FileDTO> CreateImage(CreateFileDTO model);
    }
}
