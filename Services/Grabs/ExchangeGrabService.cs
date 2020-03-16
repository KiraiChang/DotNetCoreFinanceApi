using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using FinanceApi.Cores.Extensions;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Enums;
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
                    if (items.Count > 0
                        && !string.IsNullOrEmpty(items[(int)ExchangeProps.Date].InnerText)
                        && !string.Equals(Regex.Unescape(items[(int)ExchangeProps.UsdTwd].InnerText), "-"))
                    {
                        result.InnerResult.Add(new Exchange()
                        {
                            Date = DateTime.Parse(items[(int)ExchangeProps.Date].InnerText),
                            Data = new List<ExchangeItem>()
                            {
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.UsdTwd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.UsdTwd].InnerText))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.RmbTwd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.RmbTwd].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.EurUsd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.EurUsd].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.UsdJpy.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.UsdJpy].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.GbpUsd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.GbpUsd].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.AudUsd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.AudUsd].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.UsdHkd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.UsdHkd].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.UsdRmb.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.UsdRmb].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.UsdZar.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.UsdZar].InnerText).Replace("-", "0"))
                                },
                                new ExchangeItem()
                                {
                                    Id = ExchangeProps.NzdUsd.ExtGetDescription(),
                                    Value = decimal.Parse(Regex.Unescape(items[(int)ExchangeProps.NzdUsd].InnerText).Replace("-", "0"))
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