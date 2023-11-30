using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2022
{
    public class Day25 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await Input.GetInput(2022, 25);
            ulong acc = 0;
            var maxNumLen = 0;

            foreach (var number in input.Select(line => line.ToCharArray()))
            {
                maxNumLen = Math.Max(maxNumLen, number.Length);
                for (int i = 0; i < number.Length; i++)
                {
                    var c = (ulong)(number[number.Length - 1 - i]);
                    int digit = -9999;
                    switch (c)
                    {
                        case '=':
                            digit = -2;
                            break;
                        case '-':
                            digit = -1;
                            break;
                        case '0':
                            digit = 0;
                            break;
                        case '1':
                            digit = 1;
                            break;
                        case '2':
                            digit = 2;
                            break;
                    }

                    ulong component = (ulong)(digit * Math.Pow(5, i));
                    acc += component;
                }
            }

            string Numberino(ulong number)
            {
                StringBuilder sb = new StringBuilder();
                var remainder = number;
                List<int> digits = new List<int>();

                for (int i = maxNumLen + 1; i >= 0; i--)
                {
                    var digit = (int)(remainder / Math.Pow(5, i));
                    remainder = (ulong)(number % Math.Pow(5, i));
                    digits.Add(digit);
                }

                var snafud = new List<char>();
                var nextOne = 0;
                for (int i = digits.Count - 1; i >= 0; i--)
                {
                    var digit = digits[i] + nextOne;
                    nextOne = 0;

                    while (digit > 2)
                    {
                        digit -= 5;
                        nextOne++;
                    }

                    switch (digit)
                    {
                        case -2:
                            snafud.Add('=');
                            break;
                        case -1:
                            snafud.Add('-');
                            break;
                        case 0:
                            snafud.Add('0');
                            break;
                        case 1:
                            snafud.Add('1');
                            break;
                        case 2:
                            snafud.Add('2');
                            break;
                        default:
                            throw new Exception();
                    }
                }

                snafud.Reverse();
                var ans = string.Join("", snafud.SkipWhile(c => c == '0'));

                return ans;
            }

            var x = Numberino(acc);
            Console.WriteLine(acc);
            Console.WriteLine(x);

            return x;
        }

        public async Task<object> Part2()
        {
            return await Part2();
        }
    }
}
