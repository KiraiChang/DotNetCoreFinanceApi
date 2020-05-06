using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using FinanceApi.Cores.Extensions;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Enums;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Extensions;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// Exchange grab service
    /// </summary>
    public class ExchangeGrabService : IExchangeGrabService
    {
        /// <summary>
        /// Logger for ExchangeGrabService
        /// </summary>
        private ILogger<ExchangeGrabService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeGrabService" /> class.
        /// </summary>
        /// <param name="logger">logger of ExchangeGrabService</param>
        public ExchangeGrabService(ILogger<ExchangeGrabService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IExchangeGrabService.GetList(ExchangeFilter)"/>
        public ServiceResult<IList<Exchange>> GetList(ExchangeFilter filter)
        {
            var method = MethodBase.GetCurrentMethod();
            var result = new ServiceResult<IList<Exchange>>();
            result.InnerResult = new List<Exchange>();
            if (!filter.Date.HasValue)
            {
                filter.Date = DateTime.Now;
            }

            var queryStartDate = filter.Date.Value.AddDays(-10).Date.ToString("yyyy/MM/dd");
            var queryEndDate = filter.Date.Value.Date.ToString("yyyy/MM/dd");

            var client = new RestClient("https://www.taifex.com.tw/cht/3/dailyFXRate");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", $"queryStartDate={queryStartDate.UrlEncode()}&queryEndDate={queryEndDate.UrlEncode()}", "application/x-www-form-urlencoded", ParameterType.RequestBody);
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(response.Content);
                _logger.LogTrace(response.Content);
                var count = 0;
                foreach (var row in doc.DocumentNode.SelectNodes("//*[contains(@class, \"table_c\")]/tbody/tr"))
                {
                    count++;
                    if (count == 1)
                    {
                        continue;
                    }

                    // 日期 美元／新台幣 人民幣／新台幣 歐元／美元 美元／日幣 英鎊／美元 澳幣／美元 美元／港幣 美元／人民幣 美元／南非幣 紐幣／美元
                    var items = row.SelectNodes("td");
                    if (items.Count > 0
                        && !string.IsNullOrEmpty(items[(int)ExchangeProps.Date].InnerText)
                        && !string.IsNullOrWhiteSpace(Regex.Unescape(items[(int)ExchangeProps.UsdTwd].InnerText.Replace("-", string.Empty))))
                    {
                        var item = new Exchange()
                        {
                            Date = DateTime.Parse(items[(int)ExchangeProps.Date].InnerText),
                            Data = new List<ExchangeItem>(),
                        };

                        for (var i = (int)ExchangeProps.UsdTwd; i < (int)ExchangeProps.End; i++)
                        {
                            if (!string.IsNullOrWhiteSpace(Regex.Unescape(items[i].InnerText.Replace("-", string.Empty))))
                            {
                                var value = decimal.Parse(Regex.Unescape(items[i].InnerText));
                                var subItem = new ExchangeItem()
                                {
                                    Id = ((ExchangeProps)i).ExtGetDescription(),
                                    Value = value
                                };

                                if (subItem.Value > 0)
                                {
                                    item.Data.Add(subItem);
                                }
                            }
                        }

                        if (item.Data.Count > 0)
                        {
                            result.InnerResult.Add(item);
                        }
                    }
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