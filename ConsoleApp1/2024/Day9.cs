using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2024
{
    public class Day9 : IAocDay
    {
        public const int Empty = -1;

        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2024, 9);
            var blocks = ParseIntoBlocks(input.Single());

            for (int i = 0; i < blocks.Count; i++)
            {
                if (blocks[i] == Empty)
                {
                    var lastNonEmpty = blocks.FindLastIndex(x => x != Empty);
                    if (lastNonEmpty > i)
                    {
                        SwapItems(blocks, i, lastNonEmpty);
                    }
                }
            }

            var score = Checksum(blocks);
            return score;
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2024, 9);
            var blocks = ParseIntoBlocks(input.Single());

            for (int i = blocks.Count - 1; i >= 0; i--)
            {
                if (blocks[i] != Empty)
                {
                    var fileId = blocks[i];
                    var size = 0;
                    while (i >= 0 && blocks[i] == fileId)
                    {
                        i--;
                        size++;
                    }

                    i++;
                    var startIndex = i;

                    bool done = false;
                    for (int j = 0; j < startIndex; j++)
                    {
                        if (done)
                        {
                            break;
                        }

                        if (blocks[j] == Empty)
                        {
                            var emptyIndex = j;
                            var emptySize = 0;
                            while (blocks[j] == Empty)
                            {
                                emptySize++;
                                j++;
                            }

                            if (emptySize >= size)
                            {
                                for (var k = 0; k < size; k++)
                                {
                                    SwapItems(blocks, k + startIndex, k + emptyIndex);
                                }

                                done = true;
                            }
                        }
                    }
                }
            }

            var checksum = Checksum(blocks);
            return checksum;
        }

        private static void SwapItems(List<int> list, int indexA, int indexB)
        {
            (list[indexB], list[indexA]) = (list[indexA], list[indexB]);
        }

        private long Checksum(List<int> list)
        {
            var acc = 0L;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] != Empty)
                {
                    acc += list[i] * i;
                }
            }

            return acc;
        }

        private static List<int> ParseIntoBlocks(string input)
        {
            List<int> result = new List<int>();
            bool isFileBlock = true;
            int fileId = 0;
            foreach (var c in input)
            {
                var num = int.Parse($"{c}");
                for (int i = 0; i < num; i++)
                {
                    if (isFileBlock)
                    {
                        result.Add(fileId);
                    }
                    else
                    {
                        result.Add(Empty);
                    }
                }

                if (isFileBlock)
                {
                    fileId++;
                }

                isFileBlock = !isFileBlock;
            }

            return result;
        }
    }
}
