using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Git
{
    public static class GitExtendsions
    {
        public static void ExecuteGitUploadPack(this IGitService self, string userName, string repositoryName, Stream inStream, Stream outStream)
        {
            self.ExecuteServiceByName(
                userName,
                repositoryName,
                "upload-pack",
                new ExecutionOptions() { AdvertiseRefs = false, endStreamWithClose = true },
                inStream,
                outStream);
        }

        public static void ExecuteGitReceivePack(this IGitService self, string userName, string repositoryName, Stream inStream, Stream outStream)
        {
            self.ExecuteServiceByName(
                userName,
                repositoryName,
                "receive-pack",
                new ExecutionOptions() { AdvertiseRefs = false },
                inStream,
                outStream);
        }
    }
}
