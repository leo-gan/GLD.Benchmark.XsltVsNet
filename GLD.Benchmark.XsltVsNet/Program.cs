using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLD.Benchmark.XsltVsNet
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var repetitions = int.Parse(args[0]);
            int numberOfPoliceRecords = int.Parse(args[1]);
            Console.WriteLine("Repetitions: " + repetitions);
            Console.WriteLine("Number of Police Records: " + numberOfPoliceRecords);
            var transformations = new Dictionary<string, TransformAndSerialize>
            {
                {"Net Transform (Json)", new TransformAndSerialize((new NetTransformer()).Transform, true)},
                {"Net Transform", new TransformAndSerialize((new NetTransformer()).Transform, false)},
                {"Xlst Transform", new TransformAndSerialize((new XsltTransformer()).Transform, false)},
                {"Net Enrich (Json)", new TransformAndSerialize((new NetTransformer()).Enrich, true)},
                {"Net Enrich", new TransformAndSerialize((new NetTransformer()).Enrich, false)},
                {"Xslt Enrich", new TransformAndSerialize((new XsltTransformer()).Enrich, false)},
            };

            Tester.Tests(repetitions, numberOfPoliceRecords, transformations);
        }
    }

    internal class TransformAndSerialize
    {
        public Func<string, bool, string> Transfromer { get; set; }
       public bool TryJson { get; set; }

       public TransformAndSerialize(Func<string, bool, string> transfromer, bool tryJson)
        {
            Transfromer = transfromer;
            TryJson = tryJson;
        }
    }
}