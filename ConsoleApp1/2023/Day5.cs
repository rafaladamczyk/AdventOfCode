using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;
using AoC2022;
using Range = AdventOfCode.Utils.Range;

namespace AoC2023;

public class Day5 : IAocDay
{
    public async Task<object> Part1()
    {
        var seedsToSoil = new List<List<Range>>();
        var soilToFertilizer = new List<List<Range>>();
        var fertilizerToWater = new List<List<Range>>();
        var waterToLight = new List<List<Range>>();
        var lightToTemperature = new List<List<Range>>();
        var temperatureToHumidity = new List<List<Range>>();
        var humidityToLocation = new List<List<Range>>();

        var lines = await IO.GetInput(2023, 5);
        var seeds = ParseSeeds(lines).SelectMany(x => new []{x.s, x.e - x.s});
        ParseInput(lines, seedsToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature, temperatureToHumidity, humidityToLocation);

        ulong min = int.MaxValue;
        foreach (var seed in seeds)
        {
            var soil = GetMappedNumber(seed, seedsToSoil);
            var fert = GetMappedNumber(soil, soilToFertilizer);
            var water = GetMappedNumber(fert, fertilizerToWater);
            var light = GetMappedNumber(water, waterToLight);
            var temp = GetMappedNumber(light, lightToTemperature);
            var hum = GetMappedNumber(temp, temperatureToHumidity);
            var location = GetMappedNumber(hum, humidityToLocation);

            min = Math.Min(min, location);
        }

        var ans = min;
        return ans;
    }

    public async Task<object> Part2()
    {
        var seedsToSoil = new List<List<Range>>();
        var soilToFertilizer = new List<List<Range>>();
        var fertilizerToWater = new List<List<Range>>();
        var waterToLight = new List<List<Range>>();
        var lightToTemperature = new List<List<Range>>();
        var temperatureToHumidity = new List<List<Range>>();
        var humidityToLocation = new List<List<Range>>();

        var lines = await IO.GetInput(2023, 5);
        //var lines = await IO.GetExampleInput();
        var seeds = ParseSeeds(lines);
        ParseInput(lines, seedsToSoil, soilToFertilizer, fertilizerToWater, waterToLight, lightToTemperature,
            temperatureToHumidity, humidityToLocation);

        bool bruteForce = true;

        if (bruteForce)
        {
            var sync = new object();
            ulong min = int.MaxValue;

            Parallel.ForEach(seeds.SelectMany(x => x.Enumerate()), seed =>
            {
                var soil = GetMappedNumber(seed, seedsToSoil);
                var fert = GetMappedNumber(soil, soilToFertilizer);
                var water = GetMappedNumber(fert, fertilizerToWater);
                var light = GetMappedNumber(water, waterToLight);
                var temp = GetMappedNumber(light, lightToTemperature);
                var hum = GetMappedNumber(temp, temperatureToHumidity);
                var location = GetMappedNumber(hum, humidityToLocation);

                lock (sync)
                {
                    if (location < min)
                    {
                        Console.WriteLine($"New minimum found: {location}");
                        min = location;
                    }
                }
            });
            var ans = min;

            return ans;
        }
        else
        {
                var soil = GetMappedRanges(seeds, seedsToSoil).ToList();
                var fert = GetMappedRanges(soil, soilToFertilizer).ToList();
                var water = GetMappedRanges(fert, fertilizerToWater).ToList();
                var light = GetMappedRanges(water, waterToLight).ToList();
                var temp = GetMappedRanges(light, lightToTemperature).ToList();
                var hum = GetMappedRanges(temp, temperatureToHumidity).ToList();
                var location = GetMappedRanges(hum, humidityToLocation).ToList();

                return location.Select(x => x.s).Min();
        }
    }

    private List<Range> ParseSeeds(List<string> lines)
    {
        var seeds = new List<Range>();
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("seeds:"))
            {
                var seedLine = lines[i].Split(':')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .Select(ulong.Parse).ToList();
                for (var j = 0; j < seedLine.Count; j += 2)
                {
                    seeds.Add(new Range(seedLine[j], seedLine[j] + seedLine[j + 1] - 1));
                }
            }
        }

        return seeds;
    }


    private void ParseInput(List<string> lines, List<List<Range>> seedsToSoil, List<List<Range>> soilToFertilizer,
        List<List<Range>> fertilizerToWater, List<List<Range>> waterToLight, List<List<Range>> lightToTemperature,
        List<List<Range>> temperatureToHumidity, List<List<Range>> humidityToLocation)
    {
        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i].StartsWith("seed-to-soil"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, seedsToSoil);
                    i++;
                }
            }

            if (lines[i].StartsWith("soil-to-fertilizer"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, soilToFertilizer);
                    i++;
                }
            }

            if (lines[i].StartsWith("fertilizer-to-water"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, fertilizerToWater);
                    i++;
                }
            }

            if (lines[i].StartsWith("water-to-light"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, waterToLight);
                    i++;
                }
            }

            if (lines[i].StartsWith("light-to-temperature"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, lightToTemperature);
                    i++;
                }
            }

            if (lines[i].StartsWith("temperature-to-humidity"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, temperatureToHumidity);
                    i++;
                }
            }

            if (lines[i].StartsWith("humidity-to-location"))
            {
                i++;
                while (i < lines.Count && lines[i].Trim() != string.Empty)
                {
                    var rangeInfo = lines[i].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToList();
                    ParseRangeIntoMaps(rangeInfo, humidityToLocation);
                    i++;
                }
            }
        }
    }

    private ulong GetMappedNumber(ulong item, List<List<Range>> ranges)
    {
        foreach (var map in ranges)
        {
            var src = map[0];
            var dst = map[1];

            if (src.Contains(item))
            {
                var delta = item - src.s;
                return dst.s + delta;
            }
        }

        return item;
    }

    private IEnumerable<Range> GetMappedRanges(List<Range> sourceRanges, List<List<Range>> ranges)
    {
        yield break;
        //foreach (var sourceRange in sourceRanges)
        //{
        //    foreach (var targetRange in ranges)
        //    {
        //        var src = targetRange[0];
        //        var dst = targetRange[1];

        //        if (src.Overlaps(sourceRange))
        //        {
        //            var intersect = src.Intersect(sourceRange);
        //            var delta = intersect.s - src.s;
        //            var count = intersect.e - intersect.s;

        //            var mappedTargets = new Range(dst.s + delta, dst.s + count);
        //            yield return mappedTargets;

        //            // unmapped
        //            yield return new Range(sourceRange.s, intersect.s - 1);
        //            yield return new Range(intersect.e + 1, sourceRange.e);
        //        }
        //        else
        //        {
        //            yield return sourceRange;
        //        }
        //    }
        //}
    }

    private void ParseRangeIntoMaps(IList<ulong> rangeInfo, List<List<Range>> maps)
    {
        var srcdst = new List<Range>();
        srcdst.Add(new Range(rangeInfo[1], rangeInfo[1] + rangeInfo[2] - 1));
        srcdst.Add(new Range(rangeInfo[0], rangeInfo[0] + rangeInfo[2] - 1));

        maps.Add(srcdst);
    }
}