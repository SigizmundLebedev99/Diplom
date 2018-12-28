using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
