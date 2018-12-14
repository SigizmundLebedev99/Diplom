using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Git
{
    public struct GitServiceExecutorParams
    {
        public string GitPath { get; set; }

        public string GitHomePath { get; set; }

        public string RepositoriesDirPath { get; set; }
    }
}
