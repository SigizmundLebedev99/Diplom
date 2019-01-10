using Microsoft.Extensions.Configuration;

namespace TeamEdge.BusinessLogicLayer
{
    public class PathParams
    {
        public PathParams(IConfiguration config)
        {
            RepositoriesDirPath = config.GetSection("Paths:RepoDirPath").Value;
            FileDirectoryPath = config.GetSection("Paths:FileDirPath").Value;
        }

        public string RepositoriesDirPath { get; }
        public string FileDirectoryPath { get; set; }
    }
}
