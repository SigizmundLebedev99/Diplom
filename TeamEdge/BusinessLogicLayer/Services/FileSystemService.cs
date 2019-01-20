using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class FileSystemService
    {
        //private readonly ILogger //_logger;
        private readonly PathParams _params;
        private readonly IHostingEnvironment _environment;

        public FileSystemService(PathParams parameters, IHostingEnvironment environment)
        {
            _params = parameters;
            //_logger = logger;
            _environment = environment;
        }

        public async Task<string> DocsSave(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(_params.FileDirectoryPath))
                return null;

            string ext = file.FileName.Split('.').Last();
            if (!(ext == "doc" || ext == "docx" ||
                ext == "xls" || ext == "xlsx" || ext == "pdf" || ext == "txt"))
                return null;

            string hash = GetHashFromFile(file.OpenReadStream());

            //_logger.LogDebug("Compute new doc location.");
            string dir1 = _params.FileDirectoryPath + "/" + hash.Substring(0, 2);
            string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";

            //_logger.LogDebug("Check directory existance.");
            if (!Directory.Exists(dir1))
            {
                Directory.CreateDirectory(dir1);
                Directory.CreateDirectory(dir2);
            }
            else if (!Directory.Exists(dir2))
                Directory.CreateDirectory(dir2);

            //_logger.LogDebug("Start copy doc to server.");
            string result = dir2 + file.FileName;
            using (FileStream sw = new FileStream(result, FileMode.Create))
            {
                await file.CopyToAsync(sw);
            }

            //_logger.LogDebug($"File \"{file.FileName}\" save to path \"${result}\".");

            //_logger.LogDebug("End of DocsSave action.");
            return result.Replace(_params.FileDirectoryPath, "");
        }

        public async Task<string> AvatarSave(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(_environment.WebRootPath))
                return null;

            //_logger.LogDebug($"AvatarSave action for file {file.FileName} started!");

            ////_logger.LogDebug("Check for image.");
            //Image<Rgba32> src = Image.Load(file.OpenReadStream());
            ////_logger.LogDebug("Resize image to 128x128.");
            //Image<Rgba32> image = src.Clone(x => x.Resize(new Size(128, 128)));

            ////_logger.LogDebug("Compute hash from new image.");
            string hash = GetHashFromFile(file.OpenReadStream());

            ////_logger.LogDebug("Compute new image location.");
            string dir1 = _environment.WebRootPath + "/Avatars/" + hash.Substring(0, 2);
            string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";

            ////_logger.LogDebug("Check directory existance.");
            if (!Directory.Exists(dir1))
            {
                Directory.CreateDirectory(dir1);
                Directory.CreateDirectory(dir2);
            }
            else if (!Directory.Exists(dir2))
                Directory.CreateDirectory(dir2);

            //_logger.LogDebug("Start copy file to server.");
            string result = dir2 + file.FileName;
            using(var fs = new FileStream(result, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }
            //_logger.LogDebug($"File \"{file.FileName}\" save to path \"${result}\".");

            //_logger.LogDebug("End of AvatarSave action.");
            var res = result;
            return res.Replace(_environment.WebRootPath, "");
        }

        public bool RemoveFile(string path)
        {
            try
            {
                FileInfo file = new FileInfo(_params.FileDirectoryPath + path);
                if (file.Exists)
                    file.Delete();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private string GetHashFromFile(byte[] bytes)
        {
            var hash = SHA1.Create().ComputeHash(bytes);
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }

        private string GetHashFromFile(Stream stream)
        {
            var hash = SHA1.Create().ComputeHash(stream);
            var sb = new StringBuilder(hash.Length * 2);

            foreach (byte b in hash)
            {
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
