using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Enums;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// Stock grab service
    /// </summary>
    public class StockGrabService : IStockGrabService
    {
        /// <summary>
        /// Logger for StockGrabService
        /// </summary>
        private ILogger<StockGrabService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockGrabService" /> class.
        /// </summary>
        /// <param name="logger">logger of ExchangeGrabService</param>
        public StockGrabService(ILogger<StockGrabService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IStockGrabService.GetList(StockFilter)"/>
        public ServiceResult<IList<Stock>> GetList(StockFilter filter)
        {
            var result = new ServiceResult<IList<Stock>>();
            if (!filter.BeginDate.HasValue)
            {
                filter.BeginDate = DateTime.Now.AddDays(-1);
            }

            if (!filter.EndDate.HasValue)
            {
                filter.EndDate = DateTime.Now;
            }

            if (string.IsNullOrWhiteSpace(filter.StockId))
            {
                result.ErrorMessage = "StockId is necessary";
                return result;
            }

            result.InnerResult = new List<Stock>();
            var culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            var client = new RestClient("https://www.twse.com.tw/zh/exchangeReport/STOCK_DAY");
            var request = new RestRequest(string.Empty, Method.GET);
            request.AddParameter(new Parameter("response", "json", ParameterType.QueryString));
            request.AddParameter(new Parameter("date", filter.BeginDate.Value.Date.ToString("yyyyMMdd"), ParameterType.QueryString));
            request.AddParameter(new Parameter("stockNo", filter.StockId, ParameterType.QueryString));
            var response = client.Execute<TWSEStock>(request);
            if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
            {
                foreach (var item in response.Data.data)
                {
                    try
                    {
                        var s = new Stock()
                        {
                            StockId = filter.StockId,
                            Date = DateTime.Parse(item[(int)StockProps.Date], culture),
                            OpenPrice = decimal.Parse(item[(int)StockProps.OpenPrice]),
                            MaxPrice = decimal.Parse(item[(int)StockProps.MaxPrice]),
                            MinPrice = decimal.Parse(item[(int)StockProps.MinPrice]),
                            ClosePrice = decimal.Parse(item[(int)StockProps.ClosePrice]),
                            Decline = decimal.Parse(item[(int)StockProps.Decline].Replace("X", string.Empty)),
                            Volume = long.Parse(item[(int)StockProps.Volume].Replace(",", string.Empty)),
                            Amount = long.Parse(item[(int)StockProps.Amount].Replace(",", string.Empty)),
                            Count = long.Parse(item[(int)StockProps.Count].Replace(",", string.Empty)),
                        };
                        result.InnerResult.Add(s);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, JsonConvert.SerializeObject(item));
                    }
                }

                result.IsSuccess = true;
            }

            return result;
        }

        /// <summary>
        /// 台灣證交所股票Api返還結構
        /// </summary>
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "*", Justification = "According for api result")]
        internal class TWSEStock
        {
            /// <summary>
            /// 股票資訊
            /// </summary>
            public List<List<string>> data { get; set; }
        }
    }
}