using Microsoft.AspNetCore.Http;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IFileWorkService
    {
        string SavePhoto(IFormFile file);
        string SaveFile(IFormFile file);
        bool RemovePhoto(string path);
        bool RemoveFile(string path);
    }
}
