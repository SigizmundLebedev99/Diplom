using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IFileWorkService
    {
        Task<(byte[] bytes, string type, string name)> GetFile(int fileId, int userId);
        Task<int> CreateFile(CreateFileDTO file);
    }
}
