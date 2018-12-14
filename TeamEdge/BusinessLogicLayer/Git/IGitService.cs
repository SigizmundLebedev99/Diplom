using System.IO;

namespace TeamEdge.BusinessLogicLayer.Git
{
    public interface IGitService
    {
        void ExecuteServiceByName(string userName, string correlationId, string repositoryName, string serviceName, ExecutionOptions options, Stream inStream, Stream outStream);
    }
}
