using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.UI.Popups;

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
                    
                    //var node = document.DocumentNode.SelectSingleNode("(//span)[8]");
                    //var nodeInner = node.InnerText;

                    //var pattern = @"(\d{10})";
                    //Match match = Regex.Match(nodeInner, pattern);

                    var pattern = @"((?<=CIK=)\d{10})";
                    Match match = Regex.Match(document.Text, pattern);

                    return match.Success ? match.Value : "Error on matching for a CIK number!";
                }
            }
        }

        public async Task<List<string>> GetInfo(string ticker)
        {
            var cik = await GetCIKNumberAsync(ticker);
            if (cik.Equals("Error on matching for a CIK number!"))
            {
                var messageDialog = new MessageDialog(cik);
                await messageDialog.ShowAsync();
            }
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
                        //2,928ms with new internet. New internet is faster than I'd expect the average user to have. So there's performance issues to be addressed outside of internet speed.
                    //TODO: consider removing line //var tableNodes = document.DocumentNode.SelectSingleNode("//table[@id='transaction-report']");
                    //This is considerable faster than the above, commented out line. Sometimes taking less than 1000ms
                    //Note that there is still perceivable latency in the sense that the user can click the ListPage's icon and navigate to it before the list loads. 
                    //This might be lessened further if the GetCIKNumberAsync method is optimized now
                    var tableNodes = document.DocumentNode.SelectSingleNode("(//table)[8]");
                    cells.AddRange(from row in tableNodes.SelectNodes("tr") from cell in row.SelectNodes("th|td") select cell.InnerText);
                }
            }

            return cells;
        }
    }
}