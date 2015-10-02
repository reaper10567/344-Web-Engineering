using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Xml;
using System.Text;
using System.Text.RegularExpressions;

namespace SE344.Services
{
    public interface IStockInformationService
    {
        /// <summary>
        /// Return the current price of the stock with the specified ticker symbol
        /// </summary>
        decimal currentPrice(string identifier);
    }

    public class YahooStockInformationService : IStockInformationService
    {
        public decimal currentPrice(string identifier)
        {
            StringBuilder retVal = new StringBuilder();

            //???: is this actually a rule, or just a coincidence
            var FOUR_UPPERCASE_LETTERS = new Regex("^[A-Z]{1,4}$");
            if (1 == FOUR_UPPERCASE_LETTERS.Matches(identifier).Count)
            {
                var query = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20symbol%2C%20Ask%2C%20Name%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22" + identifier + "%22)%09%09&diagnostics=false&env=http%3A%2F%2Fdatatables.org%2Falltables.env");
                var request = WebRequest.Create(query);
                var response = request.GetResponse();

                if ("text/xml; charset=UTF-8" == response.ContentType)
                {
                    var responseStream = response.GetResponseStream();
                    var xmlReader = XmlReader.Create(responseStream);
                    while (xmlReader.Read())
                    {
                        xmlReader.MoveToElement();
                        // assume only one <Ask> element in document
                        if ("Ask" == xmlReader.LocalName)
                        {
                            // basically: ReadInnerText
                            XmlReader pReader = xmlReader.ReadSubtree();
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
                    return Decimal.Parse(retVal.ToString());
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
    }
}