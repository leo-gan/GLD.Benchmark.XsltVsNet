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
        readonly XslCompiledTransform _xsltTransform;
        readonly XslCompiledTransform _xsltEnrich;
        public XsltTransformer()
        {
            _xsltTransform = new XslCompiledTransform();
            _xsltTransform.Load(@"..\..\PersonToEmployee.xsl");
            _xsltEnrich = new XslCompiledTransform();
            _xsltEnrich.Load(@"..\..\PersonToPerson.xsl");
       }
        public string Transform(string serialized, bool tryJson)
        {
            return TransformProtocol(_xsltTransform, serialized);
        }

        public string Enrich(string sourceXml, bool tryJson)
        {
            return TransformProtocol(_xsltEnrich, sourceXml);
        }

        private static string TransformProtocol(XslCompiledTransform xslCompiledTransform, string sourceXml)
        {
            var myXPathDoc = new XPathDocument(new StringReader(sourceXml));
            var sb = new StringBuilder();
            using (var s = new MemoryStream())
            using (var myWriter = new StringWriter(sb))
            {
                xslCompiledTransform.Transform(myXPathDoc, null, myWriter);
                myWriter.Flush();
                return sb.ToString();
            }
        }
    }
}
