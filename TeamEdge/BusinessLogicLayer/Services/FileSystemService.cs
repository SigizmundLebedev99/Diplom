using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class FileSystemService
    {
        //private readonly ILogger //_logger;
        private readonly PathParams _params;
        private readonly IHostingEnvironment _environment;
        private readonly IMemoryCache _cache;

        public FileSystemService(PathParams parameters, IHostingEnvironment environment, IMemoryCache cache)
        {
            _params = parameters;
            _environment = environment;
            _cache = cache;
        }

        public async Task<string> DocsSave(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(_params.FileDirectoryPath))
                return null;

            if(file.Length > (10*Math.Pow(2,10)*Math.Pow(2, 10)))
                return null;

            string hash = GetHashFromFile(file.OpenReadStream());
            string dir1 = Path.Combine(_environment.ContentRootPath, _params.FileDirectoryPath, hash.Substring(0, 2));
            string dir2 = $"{dir1}/{hash.Substring(2, 2)}/";
            if (!Directory.Exists(dir1))
            {
                Directory.CreateDirectory(dir1);
                Directory.CreateDirectory(dir2);
            }
            else if (!Directory.Exists(dir2))
                Directory.CreateDirectory(dir2);
            string result = dir2 + file.FileName;
            using (FileStream sw = new FileStream(result, FileMode.Create))
            {
                await file.CopyToAsync(sw);
            }
            return result.Replace(_environment.ContentRootPath, "");
        }

        public async Task<string> ImageSave(IFormFile file)
        {
            if (file == null || string.IsNullOrEmpty(_params.FileDirectoryPath))
                return null;

            string hash = GetHashFromFile(file.OpenReadStream());
            string ext = file.FileName.Split('.').Last();
            string result = Path.Combine(_environment.WebRootPath, "Images", $"{hash}.{ext}");

            using (var fs = new FileStream(result, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            return result.Replace(_environment.WebRootPath, "");
        }

        public async Task<string> AvatarSave(IFormFile file, int userId)
        {
            if (file == null || string.IsNullOrEmpty(_environment.WebRootPath))
                return null;

            Image<Rgba32> src = Image.Load(file.OpenReadStream());
            Image<Rgba32> image = src.Clone(e => e.Resize(128, 128));
            string hash = GetHashFromFile(file.OpenReadStream());
            string ext = file.FileName.Split('.').Last();
            string result = Path.Combine(_environment.WebRootPath, "Images","Avatars",$"{hash}.{ext}");
            var res = result.Replace(_environment.WebRootPath, "");
            if (File.Exists(result))
                return res;

            using (var fs = new FileStream(result, FileMode.Create))
            {
                await file.CopyToAsync(fs);
            }

            if (!_cache.TryGetValue(userId, out var value))
            {
                var options = new MemoryCacheEntryOptions();
                options.SetSlidingExpiration(TimeSpan.FromHours(1));
                options.PostEvictionCallbacks
                    .Add(new PostEvictionCallbackRegistration()
                    {
                        EvictionCallback = (k, v, r, s) =>
                        {
                            if(r == EvictionReason.Expired)
                                RemoveFile(v.ToString(), true);
                        }
                    });
                _cache.Set(userId, res, options);
            }
            else
            {
                string oldPath = value.ToString();
                if (oldPath != res)
                {
                    RemoveFile(value.ToString(), true);
                    _cache.Set(userId, res);
                }
            }
            return res;
        }

        public bool RemoveFile(string path, bool isStatic)
        {
            try
            {
                FileInfo file = new FileInfo((isStatic?_environment.WebRootPath:_environment.ContentRootPath) + path);
                if (file.Exists)
                    file.Delete();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Commit(int userId, string path)
        {
            string value = _cache.Get(userId)?.ToString();
            if (path == null && !string.IsNullOrEmpty(value))
                RemoveFile(value, true);
            if(value!=path)
                RemoveFile(value, true);
            _cache.Remove(userId);
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
