using System;
using System.Collections.Generic;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace InsiderTrades
{
    internal class Edgar
    {
        private static async Task<string> GetCIKNumberAsync(string ticker)
        {
            var urls = $"https://www.sec.gov/cgi-bin/browse-edgar?CIK={ticker}";
            var url = @urls;

            var client = new HttpClient();

            using (var response = await client.GetAsync(url))
            {
                using (var content = response.Content)
                {
                    var result = await content.ReadAsStringAsync();
                    var document = new HtmlDocument();
                    document.LoadHtml(result);
                    var node = document.DocumentNode.SelectSingleNode("//span/a");
                    var nodeInner = node.InnerHtml;

                    var pattern = @"(\d{10})";
                    Match match = Regex.Match(nodeInner, pattern);

                    if (match.Success)
                    {
                        return match.Value;
                    }

                    return "Error on matching for a CIK number!";
                }
            }
        }

        public async Task<List<string>> GetInfo(string ticker)
        {
            var cik = await GetCIKNumberAsync(ticker);
            var urls = $"https://www.sec.gov/cgi-bin/own-disp?action=getissuer&CIK={cik}";

            var client = new HttpClient();

            var cells = new List<string>();
            using (var response = await client.GetAsync(@urls))
            {
                using (var content = response.Content)
                {
                    var document = new HtmlWeb().Load(@urls);
                    var tableNodes = document.DocumentNode.SelectSingleNode("//table[@id='transaction-report']");
                    Console.WriteLine(tableNodes.InnerHtml);
                    foreach (var row in tableNodes.SelectNodes("tr"))
                    {
                        Console.WriteLine("row");
                        foreach (var cell in row.SelectNodes("th|td"))
                        {
                            Console.WriteLine("cell: " + cell.InnerText);

                            cells.Add(cell.InnerText);
                        }
                    }
                }
            }

            return cells;
        }
    }
}