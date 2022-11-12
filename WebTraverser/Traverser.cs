﻿using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace WebTraverser
{
    internal class Traverser
    {
        private string _website;
        private string _outputFolder;

        private ConcurrentBag<string> _visitedPages;

        public Traverser(string website, string outputFolder)
        {
            _website= website;
            _outputFolder= outputFolder;
            _visitedPages= new ConcurrentBag<string>();
        }

        public async Task Run()
        {
            _visitedPages.Add(_website);
            await TraversePage(_website);
        }

        private async Task TraversePage(string url)
        {
            var content = await DownloadPage(url);
            PersistToFile(url, content);

            var links = GetUnvisitedLinks(content);

            Task.WaitAll(links.Select(x => TraversePage(x)).ToArray());
        }

        private IEnumerable<string> GetUnvisitedLinks(string page)
        {
            var hrefPattern = @"<a href\s*=\s*(?:[""'](?<1>[^""']*)[""']|(?<1>[^>\s]+))";

            var result = new List<string>();

            Match regexMatch = Regex.Match(page, hrefPattern, RegexOptions.IgnoreCase);
            while (regexMatch.Success)
            {
                var link = regexMatch.Groups[1].Value;
                if (link.Length > 1 && link.StartsWith("/"))
                {
                    var url = $"{_website}{link}";
                    if (!_visitedPages.Contains(url))
                    {
                        result.Add(url);
                        _visitedPages.Add(url);
                    }
                }
                regexMatch = regexMatch.NextMatch();
            }
            return result;
        }

        private void PersistToFile(string url, string content)
        {
            var pagePath = $"root{url.Substring(_website.Length).TrimEnd('/')}";
            pagePath = pagePath.EndsWith(".html") ? pagePath : $"{pagePath}.html";

            var filePath = Path.Combine(_outputFolder, pagePath);
            var directoryPath = Path.GetDirectoryName(filePath);

            if (directoryPath != null && !Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.WriteAllText(filePath, content);
        }

        private async Task<string> DownloadPage(string url)
        {
            Console.WriteLine($"Downloading {url}...");

            using var client = new HttpClient();
            return await client.GetStringAsync(url);
        }
    }
}
