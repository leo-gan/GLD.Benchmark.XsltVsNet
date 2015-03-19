using System;
using System.Text;

namespace GLD.Benchmark.XsltVsNet
{
    /// <summary>
    ///     It emulates the quazy-random text generator trying to use lightweith, fast methods.
    /// </summary>
    internal class Randomizer
    {
        private const string PoolUpperCaseLetters = "QWERTYUIOPASDFGHJKLZXCVBNM"; // lenght = 26
        private const string PoolLowerCaseLetters = "qwertyuiopasdfghjklzxcvbnm"; // lenght = 26

        private const string PoolPunctuation = "                    ,,,,...!?--:;";
        // be aware of the JSON special sympols

        public static Random Rand = new Random();

        public static string Name
        {
            get { return GetCapitalChar() + Word; }
        }

        public static string Word
        {
            get
            {
                int wordLenght = GetWordLenght();
                var sb = new StringBuilder(wordLenght);
                for (int i = 0; i < wordLenght; i++)
                    sb.Append(GetChar());
                return sb.ToString();
            }
        }

        public static string Id
        {
            get
            {
                int wordLenght = GetIdLenght();
                var sb = new StringBuilder(wordLenght);
                for (int i = 0; i < wordLenght; i++)
                    sb.Append(GetDigit());
                return sb.ToString();
            }
        }

        public static string Phrase
        {
            get
            {
                int phraseLenght = GetPhraseLenght();
                var sb = new StringBuilder(phraseLenght);
                sb.Append(Name);
                for (int i = 0; i < phraseLenght; i++)
                    sb.Append(Word + GetPunctuation());
                return sb.ToString();
            }
        }

        public static DateTime GetDate(DateTime startDT, DateTime stopDT)
        {
            return startDT.AddDays(Rand.Next((stopDT - startDT).Days));
        }

        private static char GetCapitalChar()
        {
            return PoolUpperCaseLetters[Rand.Next(PoolUpperCaseLetters.Length)];
        }

        private static char GetChar()
        {
            return PoolLowerCaseLetters[Rand.Next(PoolLowerCaseLetters.Length)];
        }

        private static char GetPunctuation()
        {
            return PoolPunctuation[Rand.Next(PoolPunctuation.Length)];
        }

        private static char GetDigit()
        {
            return Rand.Next(0, 9).ToString()[0];
        }

        private static int GetWordLenght()
        {
            return Rand.Next(1, 29);
        }

        private static int GetIdLenght()
        {
            return Rand.Next(1, 10);
        }

        private static int GetPhraseLenght()
        {
            return Rand.Next(1, 20);
        }

        public static byte[] GetByteBuffer(int bufferLenght)
        {
            var buffer = new byte[bufferLenght];
            for (int i = 0; i < bufferLenght; i++)
                buffer[i] = (byte) Rand.Next(' ', '~');
                    // Space to '~' symbols cover the printable ASCII symbols
            return buffer;
        }

        /// <summary>
        ///     It writes the id in byte[] format to the beginning of the buffer.
        /// </summary>
        /// <param name="wholeBufferLenght">It could not be shorter than 4 bytes.</param>
        /// <param name="id">It might be a sequencial number for the message sequence.</param>
        /// <returns></returns>
        public static byte[] GetNumberedByteBuffer(int wholeBufferLenght, int id)
        {
            var buffer = new byte[wholeBufferLenght];
            byte[] sequencialNumberBytes = BitConverter.GetBytes(id); // gets 4 bytes
            sequencialNumberBytes.CopyTo(buffer, 0);
            byte[] randomPartOfBuffer =
                GetByteBuffer(wholeBufferLenght - sequencialNumberBytes.Length);
            randomPartOfBuffer.CopyTo(buffer, sequencialNumberBytes.Length);
            return buffer;
        }
    }
}