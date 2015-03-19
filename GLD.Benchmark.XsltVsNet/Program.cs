using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GLD.Benchmark.XsltVsNet
{
    class Program
    {
        private static void Main(string[] args)
        {

            var repetitions = int.Parse(args[0]);
            int numberOfPoliceRecords = int.Parse(args[1]);
            Console.WriteLine("Repetitions: " + repetitions);
            Console.WriteLine("Number of Police Records: " + numberOfPoliceRecords);
            var serializers = new Dictionary<string, Func<string, string>> 
            {
                {"Net Transform", (new NetTransformer()).Transform},
                {"Net Enrich", (new NetTransformer()).Enrich},
  
           };

            Tester.Tests(repetitions, numberOfPoliceRecords, serializers);
        }
    }
}
