using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Xls
{
    class XlsHeaderAttribute : Attribute
    {
        public string Header { get; }

        public XlsHeaderAttribute(string header)
        {
            Header = header;
        }
    }
}
