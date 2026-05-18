using Autofac.Features.Indexed;
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
                var originItems = _infoService.GetList(new StockInfoFilter()).ConfigureAwait(false).GetAwaiter().GetResult().InnerResult;
                var originMap = originItems.ToDictionary(x => x.Id, x => x);
                var insertItems = new List<StockInfo>();
                var map = new Dictionary<string, StockInfo>();
                foreach (var item in result.InnerResult)
                {
                    map.Add(item.Id, item);
                    if (!originMap.ContainsKey(item.Id))
                    {
                        item.IsListed = true;
                        insertItems.Add(item);
                    }
                }

                foreach (var item in originItems)
                {
                    if (!map.ContainsKey(item.Id))
                    {
                        item.IsListed = false;
                        insertItems.Add(item);
                    }
                }

                if (insertItems.Count > 0)
                {
                    for(var i = 0; i < insertItems.Count; i += MaxStockInfoInsertCount)
                    {
                        var count = Math.Min(MaxStockInfoInsertCount, insertItems.Count - i);
                        var items = insertItems.GetRange(i, count);
                        var insertResult = _infoService.Insert(items).ConfigureAwait(false).GetAwaiter().GetResult();
                        if (!insertResult.IsSuccess)
                        {
                            _logger.LogError(insertResult.InnerException, insertResult.ErrorMessage);
                        }

                        _logger.LogInformation($"{method.Name} InsertResult:{insertResult}");
                    }
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
            var results = await _infoService.GetList(new StockInfoFilter()
            {
                IsListed = true
            });
            if (results.IsSuccess)
            {
                var index = 0;
                foreach (var item in results.InnerResult)
                {
                    if (item.Id.Equals(rawId, StringComparison.OrdinalIgnoreCase))
                    {
                        await GrabByInfoAsync(item);

                        index = index + 1;
                        BackgroundJob.Schedule<StockInfoGrabSchedule>(x => x.GrabAllByIndex(index), TimeSpan.FromSeconds(3));
                        return;
                    }
                    index++; 
                }
            }
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="index">index of stock info</param>
        private async Task GrabAllByIndexAsync(int index)
        {
            var method = MethodBase.GetCurrentMethod();
            var results = await _infoService.GetList(new StockInfoFilter()
            {
                IsListed = true
            });
            if (results.IsSuccess)
            {
                if (results.InnerResult.Count > index)
                {
                    var item = results.InnerResult[index];

                    await GrabByInfoAsync(item);

                    index = index + 1;
                    BackgroundJob.Schedule<StockInfoGrabSchedule>(x => x.GrabAllByIndex(index), TimeSpan.FromSeconds(3));
                }
            }
        }

        /// <summary>
        /// grab data by stock info
        /// </summary>
        /// <param name="item">stock info</param>
        private async Task GrabByInfoAsync(StockInfo item)
        {
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
        /// <param name="index">index of stock info</param>
        public void GrabAllByIndex(int index)
        {
            GrabAllByIndexAsync(index).GetAwaiter().GetResult();
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