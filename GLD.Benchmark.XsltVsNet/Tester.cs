using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GLD.Benchmark.XsltVsNet
{
   
    internal class Tester
    {
        public static void Tests(int repetitions, int numberOfPoliceRecords,
                                 Dictionary<string, TransformAndSerialize> transformations)
        {
            var processedSizes = new Dictionary<string, long[]>();
            var measurements = new Dictionary<string, long[]>();
            foreach (var operation in transformations)
            {
                processedSizes[operation.Key] = new long[repetitions];
                measurements[operation.Key] = new long[repetitions];
            }

            // the same data for all transformations:
            var testPerson = new Person(numberOfPoliceRecords);
            string originalXml = testPerson.GetXmlString();
            string originalJson = testPerson.GetJson();
            int originalSize = 0;

            for (int i = 0; i < repetitions; i++)
                foreach (var keyValuePair in transformations)
                {
                    string original = keyValuePair.Value.TryJson ? originalJson : originalXml;
                    originalSize = original.Length;
                    var sw = Stopwatch.StartNew();

                    var processed = keyValuePair.Value.Transfromer(original, keyValuePair.Value.TryJson); // execute operation

                    sw.Stop();
                    measurements[keyValuePair.Key][i] = sw.ElapsedTicks;
                    processedSizes[keyValuePair.Key][i] = processed.Length;

                    GC.Collect(); 
                    GC.WaitForFullGCComplete();
                    GC.Collect();

                    if (i != 0) continue; // trace the first result Xml-s
                    Trace.WriteLine(keyValuePair.Key + ": " + processed); // Do not use it for big Xml !
                }
            Report.Sizes(originalSize, processedSizes);
            Report.AllResults(measurements);
        }

     
    }
}