using System.Diagnostics;
using System.IO;

namespace TeamEdge.BusinessLogicLayer.Git
{
    public class GitService : IGitService
    {
        readonly GitServiceParams _params;
        public GitService(GitServiceParams parameters)
        {
            _params = parameters;
        }

        public void ExecuteServiceByName(
            string userName,
            string repositoryName,
            string serviceName,
            ExecutionOptions options,
            Stream inStream,
            Stream outStream)
        {
            var args = serviceName + " --stateless-rpc";
            args += options.ToCommandLineArgs();
            args += " \"" + Path.Combine() + "\"";

            var info = new ProcessStartInfo(_params.GitPath, args)
            {
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                WorkingDirectory = Path.Combine(_params.RepositoriesDirPath,repositoryName)
            };

            SetHomePath(info);

            info.EnvironmentVariables.Add("AUTH_USER", userName);
            info.EnvironmentVariables.Add("REMOTE_USER", userName);

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
            info.EnvironmentVariables.Add("HOME", _params.GitHomePath);
        }
    }
}
