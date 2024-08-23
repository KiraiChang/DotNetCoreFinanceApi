using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using FinanceApi.Services.Grabs.Models.Enums;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// Gold grab service
    /// </summary>
    public class GoldGrabService : IGoldGrabService
    {
        /// <summary>
        /// Logger for GoldGrabService
        /// </summary>
        private ILogger<GoldGrabService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldGrabService" /> class.
        /// </summary>
        /// <param name="logger">logger of GoldGrabService</param>
        public GoldGrabService(ILogger<GoldGrabService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc />
        public ServiceResult<IList<Gold>> GetList(GoldFilter filter)
        {
            var method = MethodBase.GetCurrentMethod();
            var result = new ServiceResult<IList<Gold>>();
            result.InnerResult = new List<Gold>();
            if (!filter.EndDate.HasValue)
            {
                filter.EndDate = DateTime.Now.Date;
            }

            if (!filter.BeginDate.HasValue)
            {
                filter.BeginDate = filter.EndDate.Value.AddDays(-14).Date;
            }

            var url = $"https://rate.bot.com.tw/gold/csv/{filter.EndDate.Value.ToString("yyyy-MM")}/TWD/0";
            var client = new RestClient(url);
            var request = new RestRequest()
            {
                Method = Method.Get
            };
            var response = client.Execute(request);
            if (response.IsSuccessful)
            {

                if (!string.IsNullOrWhiteSpace(response.Content))
                {
                    var split = response.Content.Trim().Split("\r\n");
                    for (var i = 1; i < split.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(split[i]))
                        {
                            var values = split[i].Trim().Split(",");
                            result.InnerResult.Add(new Gold()
                            {
                                Date = DateTime.ParseExact(values[(int)GoldColumn.Date], "yyyyMMdd", CultureInfo.InvariantCulture),
                                Unit = string.Equals(values[(int)GoldColumn.Unit], "1公克") ? 1 : 0,
                                Currency = string.Equals(values[(int)GoldColumn.Currency], "新台幣 (TWD)") ? 1 : 0,
                                Bid = decimal.Parse(values[(int)GoldColumn.Bid]),
                                Ask = decimal.Parse(values[(int)GoldColumn.Ask]),
                            });
                        }
                    }
                    result.IsSuccess = true;
                }
            }
            else
            {
                _logger.LogError(result.InnerException, $"{method.Name} raise exception in RESTSharp");
            }
            return result;
        }
    }
}