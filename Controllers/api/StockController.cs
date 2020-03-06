using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Dapper;
using FinanceApi.Models;
using FinanceApi.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace FinanceApi.Controllers.Api {
    [ApiController]
    [Route ("api/[controller]")]
    public class StockController : ControllerBase {
        private readonly ILogger<StockController> _logger;

        private readonly IDbConnection _db = null;

        public StockController (ILogger<StockController> logger, IDbConnection db) {
            _logger = logger;
            _db = db;
        }

        // [HttpGet]
        // public IEnumerable<Stock> Get () {
        //     var result = new List<Stock> ();
        //     result = _db.Query<Stock> ("SELECT * FROM stocks") as List<Stock>;
        //     return result;
        // }

        [HttpGet]
        public IEnumerable<Stocks> Patch (DateTime? begin, DateTime? end) {
            if (!begin.HasValue) {
                begin = DateTime.Now;
            }

            if (!end.HasValue) {
                end = DateTime.Now;
            }

            var result = new List<Stocks> ();
            for (var input = begin.Value; input <= end; input = input.AddMonths (1)) {
                result = new List<Stocks> ();
                var culture = new CultureInfo ("zh-TW");
                culture.DateTimeFormat.Calendar = new TaiwanCalendar ();
                var client = new RestClient ("https://www.twse.com.tw/zh/exchangeReport/STOCK_DAY");
                var request = new RestRequest (string.Empty, Method.GET);
                request.AddParameter (new Parameter ("response", "json", ParameterType.QueryString));
                request.AddParameter (new Parameter ("date", input.Date.ToString ("yyyyMMdd"), ParameterType.QueryString));
                request.AddParameter (new Parameter ("stockNo", "0050", ParameterType.QueryString));
                var response = client.Get (request);
                if (response.ResponseStatus == ResponseStatus.Completed && response.IsSuccessful) {
                    var content = JsonSerializer.Deserialize<StockModel> (response.Content);
                    foreach (var item in content.data) {
                        result.Add (new Stocks () {
                            StockId = "0050",
                                Date = DateTime.Parse (item[(int) StockProps.Date], culture),
                                OpenPrice = decimal.Parse (item[(int) StockProps.OpenPrice]),
                                MaxPrice = decimal.Parse (item[(int) StockProps.MaxPrice]),
                                MinPrice = decimal.Parse (item[(int) StockProps.MinPrice]),
                                ClosePrice = decimal.Parse (item[(int) StockProps.ClosePrice]),
                                Decline = decimal.Parse (item[(int) StockProps.Decline].Replace ("X", string.Empty)),
                                Volume = long.Parse (item[(int) StockProps.Volume].Replace (",", string.Empty)),
                                Amount = long.Parse (item[(int) StockProps.Amount].Replace (",", string.Empty)),
                                Count = long.Parse (item[(int) StockProps.Count].Replace (",", string.Empty)),
                        });
                    }
                    var sql = "INSERT IGNORE INTO Stocks(StockId, Date, OpenPrice, MaxPrice, MinPrice, ClosePrice, Decline, Volume, Amount, Count) VALUES(@StockId, @Date, @OpenPrice, @MaxPrice, @MinPrice, @ClosePrice, @Decline, @Volume, @Amount, @Count)";
                    _db.Execute (sql, result);
                    _logger.LogInformation($"Current:{input}");
                }
                Task.Delay(1000).Wait();
            }

            return result;
        }
    }

    public class StockModel {
        public List<List<string>> data { get; set; }
    }
}