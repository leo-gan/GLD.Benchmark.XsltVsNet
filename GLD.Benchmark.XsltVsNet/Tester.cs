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
        public static void Tests(int repetitions, int numberOfPoliceRecords, Dictionary<string, Func<string, string>> operations)
        {
            var measurements = new Dictionary<string, long[]>();
            foreach (var operation in operations)
                measurements[operation.Key] = new long[repetitions];
            string original = (new Person(numberOfPoliceRecords)).GetXmlString(); // the same data for all operations
            for (int i = 0; i < repetitions; i++)
                foreach (var keyValuePair in operations)
                {
                    var sw = Stopwatch.StartNew();

                    var processed = keyValuePair.Value(original); // execute operation

                    sw.Stop();
                    measurements[keyValuePair.Key][i] = sw.ElapsedTicks;
                    Trace.WriteLine(keyValuePair.Key + ": " + processed);
                    Trace.WriteLine(keyValuePair.Key + ": " + sw.ElapsedTicks);
                    //List<string> errors = original.Compare(processed);
                    //errors[0] = keyValuePair.Key + errors[0];
                    //ReportErrors(errors);
                }
            ReportAllResults(measurements);
        }

        private static void ReportAllResults(Dictionary<string, long[]> measurements)
        {
            ReportTestResultHeader();
            foreach (var oneTestMeasurments in measurements)
                ReportTestResult(oneTestMeasurments);
        }

        private static void ReportTestResult(KeyValuePair<string, long[]> oneTestMeasurements)
        {
            string report = String.Format("{0, -20}  {1,9:N0} {2,11:N0}",
                oneTestMeasurements.Key,
                AverageTime(oneTestMeasurements.Value), MaxTime(oneTestMeasurements.Value)); // , AverageSize(oneTestMeasurements.Value));

            Console.WriteLine(report);
            Trace.WriteLine(report);
        }

        private static void ReportTestResultHeader()
        {
            const string header = "Serializer:          Time: Avg,    Max ticks   \n"
                                  + "===============================================";

            Console.WriteLine(header);
            Trace.WriteLine(header);
        }

        private static void ReportErrors(List<string> errors)
        {
            // Calculate the total times discarding
            // the 5% min and 5% max test times
            if (errors.Count <= 1) return;
            foreach (string error in errors)
            {
                Trace.WriteLine(error);
                Console.WriteLine(error);
            }
        }

        public static double MaxTime(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            var times = new long[measurements.Length];
            for (int i = 0; i < measurements.Length; i++)
                times[i] = measurements[i];
            var max = times.Max();
            return max;
        }

        public static double AverageTime(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            var times = new long[measurements.Length];
            for (int i = 0; i < measurements.Length; i++)
                times[i] = measurements[i];

            Array.Sort(times);
            int repetitions = times.Length;
            long totalTime = 0;
            var discardCount = (int)Math.Round(repetitions * 0.05);
            if (discardCount == 0 && repetitions > 2) discardCount = 1;
            int count = repetitions - discardCount;
            for (int i = discardCount; i < count; i++)
                totalTime += times[i];

            return ((double)totalTime) / (count - discardCount);
        }

        //public static int AverageSize(long[] measurements)
        //{
        //    if (measurements == null || measurements.Length == 0) return 0;
        //    long totalSizes = 0;
        //    for (int i = 0; i < measurements.Length; i++)
        //        totalSizes += measurements[i].Size;

        //    return (int)(totalSizes / measurements.Length);
        //}
    }
}