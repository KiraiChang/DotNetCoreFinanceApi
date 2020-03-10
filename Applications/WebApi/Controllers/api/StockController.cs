using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.Json;
using System.Threading.Tasks;
using Dapper;
using Finance.Interfaces.Services;
using FinanceApi.Models.Enums.Stocks;
using FinanceApi.Models.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;
using FinanceApi.Models.Grab;

namespace FinanceApi.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;

        private readonly IDbConnection _db = null;

        private readonly IStockService _service = null;

        public StockController(ILogger<StockController> logger, IDbConnection db, IStockService service)
        {
            _service = service;
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        public ServiceResult<IList<Stock>> Get(DateTime? date)
        {
            return _service.GetList(new StockFilter()
            {
                Date = date?.Date
            });
        }

        [HttpGet("Patch")]
        public IEnumerable<Stock> Patch(DateTime? begin, DateTime? end)
        {
            if (!begin.HasValue)
            {
                begin = DateTime.Now;
            }

            if (!end.HasValue)
            {
                end = DateTime.Now;
            }

            var result = new List<Stock>();
            for (var input = begin.Value; input <= end; input = input.AddMonths(1))
            {
                result = new List<Stock>();
                var culture = new CultureInfo("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                var client = new RestClient("https://www.twse.com.tw/zh/exchangeReport/STOCK_DAY");
                var request = new RestRequest(string.Empty, Method.GET);
                request.AddParameter(new Parameter("response", "json", ParameterType.QueryString));
                request.AddParameter(new Parameter("date", input.Date.ToString("yyyyMMdd"), ParameterType.QueryString));
                request.AddParameter(new Parameter("stockNo", "0050", ParameterType.QueryString));
                var response = client.Get(request);
                if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful)
                {
                    var content = JsonSerializer.Deserialize<TWSEStock>(response.Content);
                    foreach (var item in content.data)
                    {
                        result.Add(new Stock()
                        {
                            StockId = "0050",
                            Date = DateTime.Parse(item[(int)StockProps.Date], culture),
                            OpenPrice = decimal.Parse(item[(int)StockProps.OpenPrice]),
                            MaxPrice = decimal.Parse(item[(int)StockProps.MaxPrice]),
                            MinPrice = decimal.Parse(item[(int)StockProps.MinPrice]),
                            ClosePrice = decimal.Parse(item[(int)StockProps.ClosePrice]),
                            Decline = decimal.Parse(item[(int)StockProps.Decline].Replace("X", string.Empty)),
                            Volume = long.Parse(item[(int)StockProps.Volume].Replace(",", string.Empty)),
                            Amount = long.Parse(item[(int)StockProps.Amount].Replace(",", string.Empty)),
                            Count = long.Parse(item[(int)StockProps.Count].Replace(",", string.Empty)),
                        });
                    }
                    _logger.LogInformation($"Current:{input}, effect:{_service.Insert(result)}");
                }
                Task.Delay(1000).Wait();
            }

            return result;
        }
    }
}