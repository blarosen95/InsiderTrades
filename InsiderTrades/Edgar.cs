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

                    return match.Success ? match.Value : "Error on matching for a CIK number!";
                }
            }
        }

        public async Task<List<string>> GetInfo(string ticker)
        {
            var cik = await GetCIKNumberAsync(ticker);
            //862ms elapse from last line to next line. Internet speed might not be the issue. GetCIKNumberAsync() has less HTML document to load and even less nodes to process data from
            var urls = $"https://www.sec.gov/cgi-bin/own-disp?action=getissuer&CIK={cik}";

            //TODO: dispose of this in a using statement (want to do it after testing performance with proper internet)
            var client = new HttpClient();

            var cells = new List<string>();
            using (var response = await client.GetAsync(@urls))
            {
                using (var content = response.Content)
                {
                    var document =
                        new HtmlWeb()
                            .Load(@urls); //Most of the latency, when using phone's hot spot, was noticed here during profiling.
                    //Which is why I suspect the internet speed to be the main issue
                    //4,076ms elapse from last line to next line on hot spot.
                    var tableNodes = document.DocumentNode.SelectSingleNode("//table[@id='transaction-report']");
                    //TODO: remove these sorts of WriteLines after performing profiling against proper internet
                    Console.WriteLine(tableNodes.InnerHtml);
                    foreach (var row in tableNodes.SelectNodes("tr"))
                    {
                        Console.WriteLine("row"); //TODO delete line
                        foreach (var cell in row.SelectNodes("th|td"))
                        {
                            Console.WriteLine("cell: " + cell.InnerText); //TODO delete line

                            cells.Add(cell.InnerText);
                        }
                    }
                }
            }

            return cells;
        }
    }
}