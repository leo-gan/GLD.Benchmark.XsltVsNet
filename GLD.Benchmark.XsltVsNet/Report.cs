using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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

    internal class Report
    {
        public static void AllResults(Dictionary<string, long[]> measurements)
        {
            RawData("Raw Data:", measurements);
            ResultHeader();
            foreach (var oneTestMeasurments in measurements)
                SingleResult(oneTestMeasurments);
        }

        private static void SingleResult(KeyValuePair<string, long[]> oneTestMeasurements)
        {
            var report =
                string.Format("{0, -20} {1,7:N0} {2,8:N0} {3,8:N0} {4,9:N0} {5,10:N0}",
                    oneTestMeasurements.Key,
                    //AverageTime(oneTestMeasurements.Value, 20),
                    AverageTime(oneTestMeasurements.Value, 10),
                    AverageTime(oneTestMeasurements.Value),
                    MinTime(oneTestMeasurements.Value),
                    P99Time(oneTestMeasurements.Value),
                    MaxTime(oneTestMeasurements.Value)
                    // ,AverageSize(oneTestMeasurements.Value)
                    );

            Console.WriteLine(report);
            Trace.WriteLine(report);
        }

        private static void ResultHeader()
        {
            const string header = "Transform:     Time: Avg-90%    -100%      Min     99st%        Max\n"
                                  +
                                  "======================================++===================++++=====";

            Console.WriteLine(header);
            Trace.WriteLine(header);
        }

        private static void RawData(string operation, Dictionary<string, long[]> measurements)
        {
            var sb = new StringBuilder();
            sb.Append(operation + ":");
            foreach (var m in measurements)
            {
                sb.Append("\n\t" + m.Key + ":\n\t\t");
                for (var i = 0; i < m.Value.Length;)
                {
                    sb.Append(string.Format("{0,12:N0} ", m.Value[i++]));
                    if (i%10 == 0) sb.Append("\n\t\t");
                }
            }
            Trace.WriteLine(sb.ToString());
        }

        private static void Errors(List<string> errors)
        {
            if (errors.Count <= 1) return;
            foreach (var error in errors)
            {
                Trace.WriteLine(error);
                Console.WriteLine(error);
            }
        }

        public static void Sizes(long originalSize, Dictionary<string, long[]> processedSizes)
        {
            var sb = new StringBuilder();
            sb.Append("Result Xml document sizes:");
            foreach (var m in processedSizes)
            {
                sb.Append("\n\t" + m.Key + ":\n\t\t");
                for (int i = 0; i < m.Value.Length; )
                {
                    sb.Append(String.Format("{0,11:N0} ", m.Value[i++]));
                    if (i % 10 == 0) sb.Append("\n\t\t");
                }
            }
            Trace.WriteLine("Original Xml document size: " + originalSize);
            Trace.WriteLine(sb.ToString());
        }

        private static long P99Time(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return BottomPercent(measurements, 1).Select(m => m).LastOrDefault();
        }

        private static long MaxTime(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return measurements.Max(m => m);
        }

        private static long MinTime(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return measurements.Min(m => m);
        }

        private static IEnumerable<long> BottomPercent(long[] measurements, int discardedPercent)
        {
            if (discardedPercent == 0) return measurements;
            var take = (int) Math.Round(measurements.Length*(100 - discardedPercent)/100.0);
            return measurements.OrderBy(m => m).Take(take);
        }

        private static double AverageTime(long[] measurements, int discardedPercent = 0)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return BottomPercent(measurements, discardedPercent).Average(m => m);
        }

        private static int AverageSize(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return (int) measurements.Average(m => m);
        }
    }
}