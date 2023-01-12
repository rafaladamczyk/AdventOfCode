using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace AdventOfCode2021
{
    class Day7
    {
        public class Digit
        {
            public char[,] parts = new char[7, 6]
            {
                { ',', 'a', 'a', 'a', 'a', ',' },
                { 'b', ',', ',', ',', ',', 'c' },
                { 'b', ',', ',', ',', ',', 'c' },
                { ',', 'd', 'd', 'd', 'd', ',' },
                { 'e', ',', ',', ',', ',', 'f' },
                { 'e', ',', ',', ',', ',', 'f' },
                { ',', 'g', 'g', 'g', 'g', ',' },
            };

        }

        public static void Run()
        {
            var outputDigits = new List<string>();
            using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\2021-input-8.txt"))
            //using (var f = File.OpenRead(@"C:\Users\Raf\Downloads\example.txt"))
            {
                using (var reader = new StreamReader(f))
                {
                    while (!reader.EndOfStream)
                    {
                        var lines = reader.ReadLine().Split('|');
                        var left = lines[0];
                        var right = lines[1].Split(' ').Select(x => x.Trim());
                        outputDigits.AddRange(right);
                    }
                }
            }

            var ans = 0;
            foreach (var outputDigit in outputDigits)
            {
                var uniqueChars = outputDigit.Distinct().Count();
                if (uniqueChars == 2 || uniqueChars == 4 || uniqueChars == 3 || uniqueChars == 7)
                {
                    ans++;
                }
            }

            Console.WriteLine(ans);
        }
    }
}