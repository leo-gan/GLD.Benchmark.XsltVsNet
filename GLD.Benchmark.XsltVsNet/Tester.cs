using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GLD.Benchmark.XsltVsNet
{
   
    internal class Tester
    {
        public static void Tests(int repetitions, int numberOfPoliceRecords,
                                 Dictionary<string, Func<string, string>> operations)
        {
            var processedSizes = new Dictionary<string, long[]>();
            var measurements = new Dictionary<string, long[]>();
            foreach (var operation in operations)
            {
                processedSizes[operation.Key] = new long[repetitions];
                measurements[operation.Key] = new long[repetitions];
            }
                // the same data for all operations
            string original = (new Person(numberOfPoliceRecords)).GetXmlString();
            var originalSize = original.Length;
            for (int i = 0; i < repetitions; i++)
                foreach (var keyValuePair in operations)
                {
                    var sw = Stopwatch.StartNew();

                    var processed = keyValuePair.Value(original); // execute operation

                    sw.Stop();
                    measurements[keyValuePair.Key][i] = sw.ElapsedTicks;
                    processedSizes[keyValuePair.Key][i] = processed.Length;
                    if (i != 0) continue; // trace the first result Xml-s
                    Trace.WriteLine(keyValuePair.Key + ": " + processed);
                }
            Report.Sizes(originalSize, processedSizes);
            Report.AllResults(measurements);
        }

     
    }
}