using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using SE344.Models;

namespace SE344.Services
{
    public interface IStockInformationService
    {
        /// <summary>
        /// Populate a search result for a stock with the current asking price,
        /// day's high and low, and yearly high and low.
        /// </summary>
        /// <param name="symbol">the stock symbol</param>
        /// <returns>search result</returns>
        Task<Stock> GetQuoteAsync(Stock stock);

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Contains the Identifier to search.</param>
        /// <returns></returns>
        public async Task<Stock> GetQuoteAsync(Stock stock)
        {
            var symbol = stock.Identifier;
            if (symbol == null)
            {
                throw new ArgumentNullException();
            }

            var endpoint =
                "https://query.yahooapis.com/v1/public/yql?q=%0A%09%09%09select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22" +
                symbol +
                "%22)%0A%09%09&format=json&diagnostics=true&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=";

            try
            {
                var res = await new HttpClient().GetStringAsync(endpoint);

                var quote = JObject.Parse(res)["query"]["results"]["quote"];

                stock.CurrentPrice = (decimal?) quote["Ask"];

                if (stock.Identifier != ((string) quote["symbol"]))
                {
                    throw new InvalidOperationException("Did not recieve correct stock quote");
                }
                stock.CurrentPrice = (decimal?) quote["Ask"];
                stock.DaysHigh = (decimal?) quote["DaysHigh"];
                stock.DaysLow = (decimal?) quote["DaysLow"];
                stock.YearsHigh = (decimal?) quote["YearHigh"];
                stock.YearsLow = (decimal?) quote["YearLow"];
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                stock.CurrentPrice = null;
                stock.DaysHigh = null;
                stock.DaysLow = null;
                stock.YearsHigh = null;
                stock.YearsLow = null;
            }
            catch (System.InvalidOperationException e)
            {
                stock.CurrentPrice = null;
                stock.DaysHigh = null;
                stock.DaysLow = null;
                stock.YearsHigh = null;
                stock.YearsLow = null;
            }

            return stock;
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

            string res;
            try
            {
                res = await new HttpClient().GetStringAsync(endpoint);
            }
            catch (System.Net.Http.HttpRequestException e)
            {
                throw new UnknownTickerSymbolException(symbol, e);
            }

            try
            {
                var history = JObject.Parse(res)["query"]["results"]["quote"];
                return history.ToString();
            }
            catch (System.InvalidOperationException e)
            {
                throw new UnknownTickerSymbolException(symbol, e);
            }
        }
    }
    
    public class UnknownTickerSymbolException : Exception
    {
        public UnknownTickerSymbolException(string symbol, Exception cause)
                           : base("Unknown symbol: " + symbol, cause)
        {
        }
    }
}
