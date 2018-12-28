using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Git
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
