using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace GLD.Benchmark.XsltVsNet
{
    //internal struct Measurements
    //{
    //    public string OperationName;
    //    public long[] Times;

    //    public Measurements(string operationName, int repetitions)
    //    {
    //        OperationName = operationName;
    //        Times = new long[repetitions];
    //    }
    //}

    internal class Tester
    {
        public static void Tests(int repetitions, int numberOfPoliceRecords,
                                 Dictionary<string, Func<string, string>> operations)
        {
            var measurements = new Dictionary<string, long[]>();
            foreach (var operation in operations)
                measurements[operation.Key] = new long[repetitions];
            string original = (new Person(numberOfPoliceRecords)).GetXmlString();
                // the same data for all operations
            for (int i = 0; i < repetitions; i++)
                foreach (var keyValuePair in operations)
                {
                    var sw = Stopwatch.StartNew();

                    var processed = keyValuePair.Value(original); // execute operation

                    sw.Stop();
                    measurements[keyValuePair.Key][i] = sw.ElapsedTicks;
                    if (i != 0) continue; // trace the first result Xml-s
                    Trace.WriteLine(keyValuePair.Key + ": " + processed);
                }
            Report.AllResults(measurements);
        }

     
    }
}