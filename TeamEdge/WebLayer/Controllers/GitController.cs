using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Git;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    public class GitController : Controller
    {
        readonly IRepositoryService _repositoryService;
        readonly IGitService _gitService;
        readonly string _reposPath;

        public GitController(IRepositoryService rps, IGitService gitService, IHostingEnvironment hostingEnvironment)
        {
            _repositoryService = rps;
            _gitService = gitService;
            _reposPath = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Repos");
        }

        [HttpGet("{repositoryName}.git/info/refs")]
        public async Task<ActionResult> SecureGetInfoRefs(string repositoryName, string service)
        {
            bool isPush = string.Equals("git-receive-pack", service, StringComparison.OrdinalIgnoreCase);

            if (!RepositoryIsValid(repositoryName))
            {
                return NotFound();
            }

            var requiredLevel = isPush ? RepositoryAccessLevel.Push : RepositoryAccessLevel.Pull;
            if (_repositoryService.HasPermission(User.Id(), repositoryName, requiredLevel))
            {
                return GetInfoRefs(repositoryName, service);
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("{repositoryName}.git/git-upload-pack")]
        public ActionResult SecureUploadPack(string repositoryName)
        {
            if (!RepositoryIsValid(repositoryName))
            {
                return NotFound();
            }

            if (_repositoryService.HasPermission(User.Id(), repositoryName, RepositoryAccessLevel.Pull))
            {
                return ExecuteUploadPack(repositoryName);
            }
            else
            {
                return UnauthorizedResult();
            }
        }

        [HttpPost("{repositoryName}.git/git-receive-pack")]
        public ActionResult SecureReceivePack(string repositoryName)
        {
            if (!RepositoryIsValid(repositoryName))
            {
                return NotFound();
            }

            if (_repositoryService.HasPermission(User.Id(), repositoryName, RepositoryAccessLevel.Push))
            {
                return ExecuteReceivePack(repositoryName);
            }
            else
            {
                return UnauthorizedResult();
            }
        }

        /// <summary>
        /// This is the action invoked if you browse to a .git URL
        /// We just redirect to the repo details page, which is basically what GitHub does
        /// </summary>
        [HttpGet("{repositoryName}.git")]
        public ActionResult GitUrl(string repositoryName)
        {
            return RedirectPermanent(Url.Action("Detail", "Repository", new { id = repositoryName }));
        }

        private ActionResult ExecuteReceivePack(string repositoryName)
        {
            
            return new GitCmdResult(
                "application/x-git-receive-pack-result",
                (outStream) =>
                {
                    _gitService.ExecuteGitReceivePack(
                        Guid.NewGuid().ToString("N"),
                        repositoryName,
                        GetInputStream(disableBuffer: true),
                        outStream);
                });
        }

        private ActionResult ExecuteUploadPack(string repositoryName)
        {
            return new GitCmdResult(
                "application/x-git-upload-pack-result",
                (outStream) =>
                {
                    _gitService.ExecuteGitUploadPack(
                        Guid.NewGuid().ToString("N"),
                        repositoryName,
                        GetInputStream(),
                        outStream);
                });
        }

        private ActionResult GetInfoRefs(string repositoryName, string service)
        {
            Response.StatusCode = 200;

            string contentType = String.Format("application/x-{0}-advertisement", service);
            string serviceName = service.Substring(4);
            string advertiseRefsContent = FormatMessage(string.Format("# service={0}\n", service)) + "0000";

            return new GitCmdResult(
                contentType,
                (outStream) =>
                {
                    _gitService.ExecuteServiceByName(
                        Guid.NewGuid().ToString("N"),
                        repositoryName,
                        serviceName,
                        new ExecutionOptions() { AdvertiseRefs = true },
                        GetInputStream(),
                        outStream
                    );
                },
                advertiseRefsContent);
        }

        private ActionResult UnauthorizedResult()
        {
            Response.Headers.Clear();
            Response.Headers.Add("WWW-Authenticate", "Basic realm=\"Bonobo Git\"");

            return new StatusCodeResult(401);
        }

        private static string FormatMessage(string input)
        {
            return (input.Length + 4).ToString("X").PadLeft(4, '0') + input;
        }

        private DirectoryInfo GetDirectoryInfo(string repositoryName)
        {
            return new DirectoryInfo(Path.Combine(_reposPath, repositoryName));
        }

        private bool RepositoryIsValid(string repositoryName)
        {
            var directory = GetDirectoryInfo(repositoryName);
            var isValid = LibGit2Sharp.Repository.IsValid(directory.FullName);
            return isValid;
        }

        private Stream GetInputStream(bool disableBuffer = false)
        {
            // For really large uploads we need to get a bufferless input stream and disable the max
            // request length.
            Stream requestStream = disableBuffer ?
                Request.GetBufferlessInputStream(disableMaxRequestLength: true) :
                Request.GetBufferedInputStream();

            return Request.Headers["Content-Encoding"] == "gzip" ?
                new GZipStream(requestStream, CompressionMode.Decompress) :
                requestStream;
        }
    }
}
