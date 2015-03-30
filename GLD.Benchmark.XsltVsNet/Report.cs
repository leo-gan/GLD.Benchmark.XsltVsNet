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
            string report = String.Format("{0, -20}  {1,12:N0} {2,12:N0}",
                oneTestMeasurements.Key,
                AverageTime(oneTestMeasurements.Value), MaxTime(oneTestMeasurements.Value));
                // , AverageSize(oneTestMeasurements.Value));

            Console.WriteLine(report);
            Trace.WriteLine(report);
        }

        private static void ResultHeader()
        {
            const string header = "Serializer:           Time: Avg,     Max ticks   \n"
                                  + "=================================================";

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
                for (int i = 0; i < m.Value.Length; )
                {
                    sb.Append(String.Format("{0,12:N0} ", m.Value[i++]));
                    if (i % 10 == 0) sb.Append("\n\t\t");
                }
            }
            Trace.WriteLine(sb.ToString());
        }

        private static void Errors(List<string> errors)
        {
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
            return PrepareTimes(measurements).Max();
        }

        public static double MinTime(long[] measurements)
        {
            if (measurements == null || measurements.Length == 0) return 0;
            return PrepareTimes(measurements).Min();
        }

        private static long[] PrepareTimes(long[] measurements)
        {
            var times = new long[measurements.Length];
            for (int i = 0; i < measurements.Length; i++)
                times[i] = measurements[i];
            return times;
        }

        public static double AverageTime(long[] measurements)
        {
            // Calculate the total times discarding
            // the 5% min and 5% max test times
            if (measurements == null || measurements.Length == 0) return 0;
            var times = PrepareTimes(measurements);

            Array.Sort(times);
            int repetitions = times.Length;
            long totalTime = 0;
            var discardCount = (int) Math.Round(repetitions*0.05);
            if (discardCount == 0 && repetitions > 2) discardCount = 1;
            int count = repetitions - discardCount;
            for (int i = discardCount; i < count; i++)
                totalTime += times[i];

            return ((double) totalTime)/(count - discardCount);
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
    }
}