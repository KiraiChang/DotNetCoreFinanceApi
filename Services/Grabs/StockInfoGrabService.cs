using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Enums;
using FinanceApi.Models.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// stock info grab service
    /// </summary>
    public class StockInfoGrabService : IStockInfoGrabService
    {
        /// <summary>
        /// Link of grab stcok info
        /// </summary>
        private static string Link { get; } = "https://isin.twse.com.tw/isin/C_public.jsp?strMode=2";

        /// <summary>
        /// Escape string list
        /// </summary>
        private static IList<string> EscapeStringList { get; } = new List<string> { "有價證券代號及名稱", "股票", "上市認購(售)權證" };

        /// <summary>
        /// Logger for ExchangeGrabService
        /// </summary>
        private ILogger<StockInfoGrabService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoGrabService" /> class.
        /// </summary>
        /// <param name="logger">logger of StockInfoGrabService</param>
        public StockInfoGrabService(ILogger<StockInfoGrabService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IStockInfoGrabService.GetList"/>
        public ServiceResult<IList<StockInfo>> GetList()
        {
            var method = MethodBase.GetCurrentMethod();
            var result = new ServiceResult<IList<StockInfo>>();
            result.InnerResult = new List<StockInfo>();

            var client = new RestClient(Link);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var doc = new HtmlDocument();
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var encoding = Encoding.GetEncoding(950).GetString(response.RawBytes);
                doc.LoadHtml(encoding);
                _logger.LogTrace(encoding);
                var table = doc.DocumentNode.SelectNodes("//body//table[contains(@class, 'h4')]");
                foreach (var row in table.Elements())
                {
                    if (EscapeStringList.Any(x => row.InnerHtml.Contains(x)) || row.ChildNodes.Count < 4)
                    {
                        continue;
                    }

                    var stockInfo = new StockInfo()
                    {
                        Id = row.ChildNodes[(int)StockInfoProps.IdAndName].InnerHtml.Split("　")[0].Trim(),
                        Name = row.ChildNodes[(int)StockInfoProps.IdAndName].InnerHtml.Split("　")[1].Trim(),
                        ISINCode = row.ChildNodes[(int)StockInfoProps.ISINCode].InnerHtml.Trim(),
                        Industry = row.ChildNodes[(int)StockInfoProps.Industry].InnerHtml.Trim(),
                        Market = row.ChildNodes[(int)StockInfoProps.Market].InnerHtml.Trim(),
                        PublicDate = DateTime.Parse(row.ChildNodes[(int)StockInfoProps.PublicDate].InnerHtml),
                        CFICode = row.ChildNodes[(int)StockInfoProps.CFICode].InnerHtml.Trim(),
                        Memo = row.ChildNodes[(int)StockInfoProps.Memo].InnerHtml.Trim(),
                    };
                    if (stockInfo.Id.Length > 5)
                    {
                        continue;
                    }

                    result.InnerResult.Add(stockInfo);
                }
            }
            else
            {
                _logger.LogError(result.InnerException, $"{method.Name} raise exception in RESTSharp");
            }

            result.IsSuccess = true;
            return result;
        }
    }
}