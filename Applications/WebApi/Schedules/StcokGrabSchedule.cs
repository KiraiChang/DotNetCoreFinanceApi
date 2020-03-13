using System;
using Finance.Interfaces.Services;
using Finance.Interfaces.Services.Grabs;
using Microsoft.Extensions.Logging;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of stock grab
    /// </summary>
    public class StcokGrabSchedule
    {
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
        private readonly ILogger<StcokGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StcokGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabService">grab service</param>
        /// <param name="service">stock service</param>
        /// <param name="logger">logger of stock grab schedule</param>
        public StcokGrabSchedule(IStockGrabService grabService, IStockService service, ILogger<StcokGrabSchedule> logger)
        {
            _service = service;
            _grabService = grabService;
            _logger = logger;
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="stockId">stockId</param>
        public void Grab(string stockId)
        {
            Grab(DateTime.Now, stockId);
        }

        /// <summary>
        /// grab stock
        /// </summary>
        /// <param name="date">date</param>
        /// <param name="stockId">stockId</param>
        public void Grab(DateTime date, string stockId)
        {
            var result = _grabService.GetList(new FinanceApi.Models.Filter.StockFilter()
            {
                Date = date.Date,
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