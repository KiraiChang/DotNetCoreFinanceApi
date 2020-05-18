using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using FinanceApi.Interfaces.Services;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of stock grab
    /// </summary>
    public class StcokInfoGrabSchedule
    {
        /// <summary>
        /// Min date of stock
        /// </summary>
        private static DateTime MinDate { get; } = new DateTime(2010, 1, 4);

        /// <summary>
        /// Wait Grab Second
        /// </summary>
        private static int WaitGrabSecond { get; } = 3;

        /// <summary>
        /// grab service
        /// </summary>
        private readonly IStockInfoGrabService _infoGrabService = null;

        /// <summary>
        /// stock service
        /// </summary>
        private readonly IStockInfoService _infoService = null;

        /// <summary>
        /// grab service
        /// </summary>
        private readonly IStockGrabService _grabService = null;

        /// <summary>
        /// stock service
        /// </summary>
        private readonly IStockService _service = null;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<StcokInfoGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StcokInfoGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabInfoService">grab stock info service</param>
        /// <param name="infoService">stock info service</param>
        /// <param name="grabService">grab service</param>
        /// <param name="service">stock service</param>
        /// <param name="logger">logger of stock grab schedule</param>
        public StcokInfoGrabSchedule(IStockInfoGrabService grabInfoService,
            IStockInfoService infoService,
            IStockGrabService grabService,
            IStockService service,
            ILogger<StcokInfoGrabSchedule> logger)
        {
            _infoService = infoService;
            _infoGrabService = grabInfoService;
            _service = service;
            _grabService = grabService;
            _logger = logger;
        }

        /// <summary>
        /// grab stock info
        /// </summary>
        public void GrabInfo()
        {
            var method = MethodBase.GetCurrentMethod();
            var result = _infoGrabService.GetList();
            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                var insertItems = new List<StockInfo>();
                var count = 0;
                foreach (var item in result.InnerResult)
                {
                    insertItems.Add(item);
                    if (insertItems.Count >= 50)
                    {
                        count += _infoService.Insert(insertItems).InnerResult;
                        insertItems.Clear();
                    }
                }

                if (insertItems.Count > 0)
                {
                    count += _infoService.Insert(insertItems).InnerResult;
                    insertItems.Clear();
                }

                _logger.LogInformation($"{method.Name} InsertCount:{count}");
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        public void Grab()
        {
            var results = _infoService.GetList();
            if (results.IsSuccess)
            {
                foreach (var item in results.InnerResult)
                {
                    Grab(DateTime.Now, item.Id);
                    Task.Delay(TimeSpan.FromSeconds(WaitGrabSecond));
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="begin">begin stock id</param>
        /// <param name="end">end stock id</param>
        public void GrabAll(int begin, int end)
        {
            var method = MethodBase.GetCurrentMethod();
            var results = _infoService.GetList();
            if (results.IsSuccess)
            {
                foreach (var item in results.InnerResult)
                {
                    var stockId = int.Parse(item.Id.Substring(0, 4));
                    if (stockId >= begin && stockId <= end)
                    {
                        for (var date = item.PublicDate; date < DateTime.Now; date = date.AddMonths(1))
                        {
                            if (date > MinDate)
                            {
                                Grab(date, item.Id);
                                Task.Delay(TimeSpan.FromSeconds(WaitGrabSecond));
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="stockId">stockId</param>
        private void Grab(DateTime date, string stockId)
        {
            var result = _grabService.GetList(new FinanceApi.Models.Filter.StockFilter()
            {
                BeginDate = date.Date.AddDays(-1),
                EndDate = date.Date,
                StockId = stockId
            });
            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                var insertResult = _service.Insert(result.InnerResult);
                _logger.LogInformation($"StockId:{stockId}, Date:{date.Date}, InsertCount:{insertResult}");
            }
        }
    }
}