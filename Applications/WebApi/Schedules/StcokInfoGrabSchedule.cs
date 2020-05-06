using System;
using System.Reflection;
using FinanceApi.Interfaces.Services;
using FinanceApi.Interfaces.Services.Grabs;
using Microsoft.Extensions.Logging;

namespace WebApi.Schedules
{
    /// <summary>
    /// schedule of stock grab
    /// </summary>
    public class StcokInfoGrabSchedule
    {
        /// <summary>
        /// grab service
        /// </summary>
        private readonly IStockInfoGrabService _grabService = null;

        /// <summary>
        /// stock service
        /// </summary>
        private readonly IStockInfoService _service = null;

        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<StcokInfoGrabSchedule> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StcokInfoGrabSchedule" /> class.
        /// </summary>
        /// <param name="grabService">grab service</param>
        /// <param name="service">stock service</param>
        /// <param name="logger">logger of stock grab schedule</param>
        public StcokInfoGrabSchedule(IStockInfoGrabService grabService, IStockInfoService service, ILogger<StcokInfoGrabSchedule> logger)
        {
            _service = service;
            _grabService = grabService;
            _logger = logger;
        }

        /// <summary>
        /// grab stock info
        /// </summary>
        public void Grab()
        {
            var method = MethodBase.GetCurrentMethod();
            var result = _grabService.GetList();
            if (result.IsSuccess && result.InnerResult.Count > 0)
            {
                //var insertResult = _service.Insert(result.InnerResult);
                //_logger.LogInformation($"{method.Name} InsertCount:{insertResult}");
            }
        }
    }
}