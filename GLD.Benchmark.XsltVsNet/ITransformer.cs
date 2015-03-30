using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLD.Benchmark.XsltVsNet
{
    interface ITransformer
    {
        string Transform(string serialized, bool tryJson);
        string Enrich(string sourceXml, bool tryJson);
    }
}
