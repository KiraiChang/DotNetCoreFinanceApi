using FinanceApi.Interfaces.Services;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of stock grab
    /// </summary>
    public class StockInfoGrabSchedule
    {
        /// <summary>
        /// Min date of stock
        /// </summary>
        private static DateTime MinDate { get; } = new DateTime(2010, 1, 4);

        /// <summary>
        /// get all stock id
        /// </summary>
        private static int MinGrabAllDataId { get; } = 50;

        /// <summary>
        /// Wait Grab Second
        /// </summary>
        private static int WaitGrabSecond { get; } = 5;

        /// <summary>
        /// Max stock count to insert
        /// </summary>
        private static int MaxStockInsertCount { get; } = 300;

        /// <summary>
        /// Max stock info count to insert
        /// </summary>
        private static int MaxStockInfoInsertCount { get; } = 50;

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
        private readonly ILogger<StockInfoGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabInfoService">grab stock info service</param>
        /// <param name="infoService">stock info service</param>
        /// <param name="grabService">grab service</param>
        /// <param name="service">stock service</param>
        /// <param name="logger">logger of stock grab schedule</param>
        public StockInfoGrabSchedule(IStockInfoGrabService grabInfoService,
            IStockInfoService infoService,
            IStockGrabService grabService,
            IStockService service,
            ILogger<StockInfoGrabSchedule> logger)
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
                foreach (var item in result.InnerResult)
                {
                    insertItems.Add(item);
                    if (insertItems.Count >= MaxStockInfoInsertCount)
                    {
                        var insertResult = _infoService.Insert(insertItems).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!insertResult.IsSuccess)
                        {
                            _logger.LogError(insertResult.InnerException, insertResult.ErrorMessage);
                        }

                        _logger.LogInformation($"{method.Name} InsertResult:{insertResult}");
                        insertItems.Clear();
                    }
                }

                if (insertItems.Count > 0)
                {
                    var insertResult = _infoService.Insert(insertItems).ConfigureAwait(false).GetAwaiter().GetResult();
                    if (!insertResult.IsSuccess)
                    {
                        _logger.LogError(insertResult.InnerException, insertResult.ErrorMessage);
                    }

                    _logger.LogInformation($"{method.Name} InsertResult:{insertResult}");
                    insertItems.Clear();
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        public void Grab()
        {
            Grab(DateTime.Now);
        }

        /// <summary>
        /// grab stock
        /// <param name="now">special date</param>
        /// </summary>
        public void Grab(DateTime date)
        {
            GrabAsync(date).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// grab stock
        /// <param name="now">special date</param>
        /// </summary>
        private async Task GrabAsync(DateTime now)
        {
            var results = await _infoService.GetList();
            if (results.IsSuccess)
            {
                var list = new List<Stock>();
                foreach (var item in results.InnerResult)
                {
                    list.AddRange(Grab(now, item.Id));
                    await Task.Delay(TimeSpan.FromSeconds(WaitGrabSecond));

                    if (list.Count > MaxStockInsertCount)
                    {
                        var insertResult = await _service.Insert(list);
                        if (!insertResult.IsSuccess)
                        {
                            _logger.LogError(insertResult.InnerException, insertResult.ErrorMessage);
                        }

                        _logger.LogInformation($"InsertResult:{insertResult}");
                        list.Clear();
                    }
                }

                if (list.Count > 0)
                {
                    var insertResult = await _service.Insert(list);
                    if (!insertResult.IsSuccess)
                    {
                        _logger.LogError(insertResult.InnerException, insertResult.ErrorMessage);
                    }

                    _logger.LogInformation($"InsertResult:{insertResult}");
                    list.Clear();
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="rawId">stock id</param>
        private async Task GrabAllAsync(string rawId)
        {
            var method = MethodBase.GetCurrentMethod();
            var stockId = 1;
            if (!string.IsNullOrWhiteSpace(rawId))
            {
                stockId = int.Parse(rawId);
            }

            var results = await _infoService.GetList();
            if (results.IsSuccess)
            {
                var last = results.InnerResult.FirstOrDefault(x => int.Parse(x.Id.Substring(0, 4)) >= stockId);
                if (last != null)
                {
                    stockId = int.Parse(last.Id.Substring(0, 4));
                    var item = results.InnerResult.FirstOrDefault(x => x.Id.Contains(last.Id.Substring(0, 4)));

                    var olds = new List<Stock>();
                    var oldResult = await _service.GetList(new StockFilter()
                    {
                        StockId = item.Id,
                        BeginDate = item.PublicDate,
                        EndDate = DateTime.Now,
                    });
                    if (oldResult.IsSuccess)
                    {
                        olds = oldResult.InnerResult as List<Stock>;
                    }

                    if (olds.Count <= 0)
                    {
                        var list = new List<Stock>();
                        for (var date = item.PublicDate; date < DateTime.Now; date = date.AddMonths(1))
                        {
                            if (date > MinDate)
                            {
                                list.AddRange(Grab(date, item.Id));
                                await Task.Delay(TimeSpan.FromSeconds(WaitGrabSecond));
                                if (list.Count > MaxStockInsertCount)
                                {
                                    var insertResult = _service.Insert(list);
                                    _logger.LogInformation($"StockId:{item.Id} InsertResult:{insertResult}");
                                    list.Clear();
                                }
                            }
                        }

                        if (list.Count > 0)
                        {
                            var insertResult = _service.Insert(list);
                            _logger.LogInformation($"StockId:{item.Id} InsertResult:{insertResult}");
                            list.Clear();
                        }
                    }
                    else
                    {
                        var months = olds.GroupBy(x => x.Date.ToString("yyyy-MM")).ToDictionary(x => x.Key, x => x.GetEnumerator());
                        var date = item.PublicDate;
                        for (; date < DateTime.Now; date = date.AddMonths(1))
                        {
                            await CheckAndInsert(item, months, date);
                        }
                        if (date.Month <= DateTime.Now.Month)
                        {
                            await CheckAndInsert(item, months, date);
                        }
                    }

                    stockId = stockId + 1;
                    last = results.InnerResult.FirstOrDefault(x => int.Parse(x.Id.Substring(0, 4)) >= stockId);
                    BackgroundJob.Schedule<StockInfoGrabSchedule>(x => x.GrabAll(last.Id.ToString()), TimeSpan.FromSeconds(3));
                }
            }

            async Task CheckAndInsert(StockInfo item, Dictionary<string, IEnumerator<Stock>> months, DateTime date)
            {
                if (date > MinDate && !months.ContainsKey(date.Date.ToString("yyyy-MM")))
                {
                    var result = Grab(date, item.Id);
                    var insertResult = await _service.Insert(result);
                    await Task.Delay(TimeSpan.FromSeconds(WaitGrabSecond));
                    _logger.LogInformation($"StockId:{item.Id}, Date:{date}, InsertResult:{insertResult}");
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="rawId">stock id</param>
        public void GrabAll(string rawId)
        {
            GrabAllAsync(rawId).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="stockId">stockId</param>
        /// <returns>list of stock</returns>
        private IList<Stock> Grab(DateTime date, string stockId)
        {
            var result = _grabService.GetList(new FinanceApi.Models.Filter.StockFilter()
            {
                BeginDate = date.Date.AddDays(-1),
                EndDate = date.Date,
                StockId = stockId
            });
            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                return result.InnerResult;
            }

            return new List<Stock>();
        }
    }
}