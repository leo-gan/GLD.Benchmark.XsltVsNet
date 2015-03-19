using System;
using System.Collections.Generic;

namespace GLD.Benchmark.XsltVsNet 
{
    internal class TestBuffer
    {
        public static byte[] GetBuffer(int bufferSize)
        {
            return Randomizer.GetByteBuffer(bufferSize);
        }

        public static byte[] GetBuffer(int bufferSize, int id)
        {
            return Randomizer.GetNumberedByteBuffer(bufferSize, id);
        }

        public static List<string> Compare(string queueName, byte[] original,  byte[] result)
        {
            var errors = new List<string> {queueName + ":  ************** Comparison failed! "};
            if (result == null)
            {
                errors.Add("\tcomparable: is null!"); 
                return errors;
            }

            if (original.Length != result.Length)
            {
                errors.Add(String.Format("\toriginal.Length({0}) != result.Length({1})",
                    original.Length, result.Length));
                return errors;
            }

            for (int i = 0; i < original.Length; i++)
                if (original[i] != result[i])
                {
                    errors.Add(String.Format("\toriginal[{0}]:{1} != result[{0}]:{2}",
                        i, original[i], result[i]));
                    break;
                }
            return errors;
        }
    }
}