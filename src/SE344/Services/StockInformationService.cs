using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using SE344.ViewModels.Stock;

namespace SE344.Services
{
    public interface IStockInformationService
    {
        /// <summary>
        /// Return the current price of the stock with the specified ticker symbol
        /// </summary>
        Task<decimal> CurrentPrice(string identifier);

        /// <summary>
        /// Populate a search result for a stock with the current asking price,
        /// day's high and low, and yearly high and low.
        /// </summary>
        /// <param name="symbol">the stock symbol</param>
        /// <returns>search result</returns>
        Task<SearchViewModel> GetQuoteAsync(string symbol);

        /// <summary>
        /// Get historical data for a stock
        /// </summary>
        /// <param name="symbol">the stock symbol</param>
        /// <param name="start">the first date to get stock data</param>
        /// <param name="end">the last date to get stock data</param>
        /// <returns>historical data as a string</returns>
        Task<string> GetHistoryInfoAsync(string symbol, DateTime start, DateTime end);
    }

    public class YahooStockInformationService : IStockInformationService
    {
        public async Task<decimal> CurrentPrice(string identifier)
        {
            var retVal = new StringBuilder();

            //???: is this actually a rule, or just a coincidence
            var fourUppercaseLetters = new Regex("^[A-Za-z]{1,4}$");
            if (1 == fourUppercaseLetters.Matches(identifier).Count)
            {
                var query = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20symbol%2C%20Ask%2C%20Name%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22" + identifier + "%22)%09%09&diagnostics=false&env=http%3A%2F%2Fdatatables.org%2Falltables.env");
                var request = WebRequest.Create(query);
                var response = await request.GetResponseAsync();

                if ("text/xml; charset=UTF-8" == response.ContentType)
                {
                    var responseStream = response.GetResponseStream();

                    if (responseStream != null)
                    {
                        var xmlReader = XmlReader.Create(responseStream);
                        while (xmlReader.Read())
                        {
                            xmlReader.MoveToElement();
                            // assume only one <Ask> element in document
                            if ("Ask" != xmlReader.LocalName) continue;

                            // basically: ReadInnerText
                            var pReader = xmlReader.ReadSubtree();
                            while (pReader.Read())
                            {
                                if (pReader.NodeType == XmlNodeType.Text)
                                {
                                    retVal.Append(pReader.Value);
                                }
                            }
                        }
                    }
                    response.Close();
                    return decimal.Parse(retVal.ToString());
                }
                else
                {
                    throw new ApplicationException("response from external API was not of expected type");
                }
            }
            else
            {
                throw new FormatException("Invalid Ticker Symbol");
            }
        }

        public async Task<SearchViewModel> GetQuoteAsync(string symbol)
        {
            if (symbol == null)
            {
                throw new ArgumentNullException();
            }

            var endpoint =
                "https://query.yahooapis.com/v1/public/yql?q=%0A%09%09%09select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22" +
                symbol +
                "%22)%0A%09%09&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

            var res = await new HttpClient().GetStringAsync(endpoint);

            var quote = JObject.Parse(res)["query"]["results"]["quote"];
            var stockModel = new SearchViewModel
            {
                Symbol = (string) quote["symbol"],
                CurrentPrice = (decimal?) quote["Ask"],
                DaysHigh = (decimal?) quote["DaysHigh"],
                DaysLow = (decimal?) quote["DaysLow"],
                YearsHigh = (decimal?) quote["YearHigh"],
                YearsLow = (decimal?) quote["YearLow"]
            };

            return stockModel;
        }

        public async Task<string> GetHistoryInfoAsync(string symbol, DateTime start, DateTime end)
        {
            if (symbol == null || start == null || end == null)
            {
                throw new ArgumentNullException();
            }

            if (start > end)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Start date is after end date");
            }

            var endpoint =
                "https://query.yahooapis.com/v1/public/yql?q=SELECT%20*%20FROM%20yahoo.finance.historicaldata%20WHERE%20symbol%3D%22" +
                symbol + "%22%20and%20startDate%3D%22" + start.Date.ToString("u") + "%22%20and%20endDate%3D%22" +
                end.Date.ToString("u") +
                "%22&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

            var res = await new HttpClient().GetStringAsync(endpoint);

            var history = JObject.Parse(res)["query"]["results"]["quote"];
            return history.ToString();
        }
    }
}
