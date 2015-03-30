using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace GLD.Benchmark.XsltVsNet
{
    class XsltTransformer : ITransformer
    {
        readonly XslCompiledTransform _xslt ;
        public XsltTransformer()
        {
             _xslt = new XslCompiledTransform();
        }
        public string Transform(string serialized, bool tryJson)
        {
            _xslt.Load(@"..\..\PersonToEmployee.xsl");
            return TransformProtocol(serialized);
        }

        public string Enrich(string sourceXml, bool tryJson)
        {
            _xslt.Load(@"..\..\PersonToPerson.xsl");
            return TransformProtocol(sourceXml);
        }

        private string TransformProtocol(string sourceXml)
        {
            var myXPathDoc = new XPathDocument(new StringReader(sourceXml));
            var sb = new StringBuilder();
            using (var s = new MemoryStream())
            using (var myWriter = new StringWriter(sb))
            {
                _xslt.Transform(myXPathDoc, null, myWriter);
                myWriter.Flush();
                return sb.ToString();
            }
        }
    }
}
