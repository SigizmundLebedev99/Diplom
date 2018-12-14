using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;

namespace TeamEdge.BusinessLogicLayer.Git
{
    public class GitService : IGitService
    {
        private readonly string gitPath;
        private readonly string gitHomePath;
        private readonly string repositoriesDirPath;
        private readonly string repoLocator;

        public GitService(GitServiceExecutorParams parameters, IHostingEnvironment hostingEnvironment)
        {
            this.gitPath = parameters.GitPath;
            this.gitHomePath = parameters.GitHomePath;
            this.repositoriesDirPath = parameters.RepositoriesDirPath;
            this.repoLocator = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Repos");
        }

        public void ExecuteServiceByName(
            string userName,
            string correlationId,
            string repositoryName,
            string serviceName,
            ExecutionOptions options,
            Stream inStream,
            Stream outStream)
        {
            var args = serviceName + " --stateless-rpc";
            args += options.ToCommandLineArgs();
            args += " \"" + Path.Combine() + "\"";

            var info = new ProcessStartInfo(gitPath, args)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.GetDirectoryName(repositoriesDirPath),
            };

            SetHomePath(info);

            var username = userName;
            var teamsstr = "";
            var rolesstr = "";
            var displayname = "";
            info.EnvironmentVariables.Add("AUTH_USER", username);
            info.EnvironmentVariables.Add("REMOTE_USER", username);

            using (var process = Process.Start(info))
            {
                inStream.CopyTo(process.StandardInput.BaseStream);
                if (options.endStreamWithClose)
                {
                    process.StandardInput.Close();
                }
                else
                {
                    process.StandardInput.Write('\0');
                }

                process.StandardOutput.BaseStream.CopyTo(outStream);
                process.WaitForExit();
            }
        }

        private void SetHomePath(ProcessStartInfo info)
        {
            if (info.EnvironmentVariables.ContainsKey("HOME"))
            {
                info.EnvironmentVariables.Remove("HOME");
            }
            info.EnvironmentVariables.Add("HOME", gitHomePath);
        }
    }
}
