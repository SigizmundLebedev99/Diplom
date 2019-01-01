using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace TeamEdge.BusinessLogicLayer
{
    public class GitServiceParams
    {
        public GitServiceParams(IHostingEnvironment env)
        {
            GitPath = Path.Combine(env.ContentRootPath, "AppData", "Git", "git.exe");
            GitHomePath = Path.Combine(env.ContentRootPath, "AppData");
            RepositoriesDirPath = Path.Combine(env.ContentRootPath, "AppData", "Repos");
        }

        public string GitPath { get; }

        public string GitHomePath { get; }

        public string RepositoriesDirPath { get; }
    }
}
