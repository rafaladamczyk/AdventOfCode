﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using File = System.IO.File;

namespace AdventOfCode.Utils
{
    public static class Input
    {
        public static async Task<List<string>> GetInput(int year, int day)
        {
            using var stream = await GetInputStream(year, day);
            return SplitIntoLines(stream).ToList();
        }

        public static async Task<List<string>> GetExampleInput()
        {
            using var stream = await GetExampleStream();
            return SplitIntoLines(stream).ToList();
        }

        private static IEnumerable<string> SplitIntoLines(Stream stream)
        {
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                yield return reader.ReadLine();
            }
        }

        private static async Task DownloadInput(string session, string localFileName, int year, int day)
        {
            if (string.IsNullOrWhiteSpace(session))
            {
                throw new Exception("no session in ENV");
            }

            var baseUri = new Uri("https://adventofcode.com");
            var cookieContainer = new CookieContainer();
            cookieContainer.Add(baseUri, new Cookie("session", session));

            using var handler = new HttpClientHandler { CookieContainer = cookieContainer };
            using var httpClient = new HttpClient(handler) { BaseAddress = baseUri };
            using var newFile = File.Create(localFileName);
            var request = new HttpRequestMessage(HttpMethod.Get, $"/{year}/day/{day}/input");
            var response = await httpClient.SendAsync(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"Response code was {response.StatusCode}. Outdated session cookie?");
            }

            using var contentStream = await response.Content.ReadAsStreamAsync();
            await contentStream.CopyToAsync(newFile);
        }

        public static async Task<Stream> GetInputStream(int year, int day)
        {
            const string topLevelPath = "../../../";
            var inputsDirectory = Directory.CreateDirectory(Path.Combine(topLevelPath, "Inputs", year.ToString()));
            var localFileName = Path.Combine(inputsDirectory.FullName, $"{day}.txt");
            var session = Environment.GetEnvironmentVariable("aoc_session");

            var localFile = new FileInfo(localFileName);

            if (!localFile.Exists || localFile.Length == 0)
            {
                await DownloadInput(session, localFileName, year, day);
            }

            return File.OpenRead(localFileName);
        }

        public static async Task<Stream> GetExampleStream()
        {
            const string topLevelPath = "../../../";
            var inputsDirectory = Directory.CreateDirectory(Path.Combine(topLevelPath, "Inputs"));
            return File.OpenRead(Path.Combine(inputsDirectory.FullName, "ExampleInput.txt"));
        }
    }
}
