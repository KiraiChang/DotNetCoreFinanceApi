using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Finance.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using HtmlAgilityPack;
using RestSharp;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// Exchange grab service
    /// </summary>
    public class ExchangeGrabService : IExchangeGrabService
    {
        /// <inheritdoc cref="IExchangeGrabService.GetList(ExchangeFilter)"/>
        public ServiceResult<IList<Exchange>> GetList(ExchangeFilter filter)
        {
            var result = new ServiceResult<IList<Exchange>>();
            result.InnerResult = new List<Exchange>();
            if (!filter.Date.HasValue)
            {
                filter.Date = DateTime.Now;
            }

            var client = new RestClient("http://www.taifex.com.tw/cht/3/dailyFXRate");
            var request = new RestRequest(string.Empty, Method.POST);
            request.AddParameter(new Parameter("queryStartDate", filter.Date.Value.Date.ToString("yyyyMMdd"), ParameterType.RequestBody));
            request.AddParameter(new Parameter("queryEndDate", filter.Date.Value.Date.ToString("yyyyMMdd"), ParameterType.RequestBody));
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(response.Content);
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
                    if (items.Count > 0 && !string.IsNullOrEmpty(items[0].InnerText) && !string.Equals(Regex.Unescape(items[1].InnerText), "-"))
                    {
                        result.InnerResult.Add(new Exchange()
                        {
                            Date = DateTime.Parse(items[0].InnerText),
                            Data = new List<ExchangeItem>()
                            {
                                new ExchangeItem()
                                {
                                    Id = "USD/TWD",
                                    Value = decimal.Parse(Regex.Unescape(items[1].InnerText))
                                },
                                new ExchangeItem()
                                {
                                    Id = "RMB/TWD",
                                    Value = decimal.Parse(Regex.Unescape(items[2].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "EUR/USD",
                                    Value = decimal.Parse(Regex.Unescape(items[3].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "USD/JPY",
                                    Value = decimal.Parse(Regex.Unescape(items[4].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "GBP/USD",
                                    Value = decimal.Parse(Regex.Unescape(items[5].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "AUD/USD",
                                    Value = decimal.Parse(Regex.Unescape(items[6].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "USD/HKD",
                                    Value = decimal.Parse(Regex.Unescape(items[7].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "USD/RMB",
                                    Value = decimal.Parse(Regex.Unescape(items[8].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "USD/ZAR",
                                    Value = decimal.Parse(Regex.Unescape(items[9].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = "NZD/USD",
                                    Value = decimal.Parse(Regex.Unescape(items[10].InnerText).Replace("-", "0"))
                                },
                            }
                        });
                    }
                }
            }

            result.IsSuccess = true;
            return result;
        }
    }
}