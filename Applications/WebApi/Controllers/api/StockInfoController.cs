using System;
using System.Collections.Generic;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Schedules;

namespace FinanceApi.Controllers.Api
{
    /// <summary>
    /// Stock Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StockInfoController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<StockInfoController> _logger;

        /// <summary>
        /// Service
        /// </summary>
        private readonly IStockInfoService _service = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoController" /> class.
        /// </summary>
        /// <param name="logger">stock controller logger</param>
        /// <param name="service">stock service</param>
        public StockInfoController(ILogger<StockInfoController> logger, IStockInfoService service)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get stock info list
        /// </summary>
        /// <returns>list of stock info</returns>
        [HttpGet]
        public ServiceResult<IList<StockInfo>> Get()
        {
            var result = _service.GetList();
            if (!result.IsSuccess)
            {
                _logger.LogError(result.InnerException, result.ErrorMessage);
            }

            return new ServiceResult<IList<StockInfo>>()
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                InnerResult = result.InnerResult,
                IsSuccess = result.IsSuccess
            };
        }

        /// <summary>
        /// Get stock info list
        /// </summary>
        /// <returns>list of stock info</returns>
        [HttpGet("Grab")]
        public void Grab()
        {
            BackgroundJob.Schedule<StcokInfoGrabSchedule>(x => x.GrabInfo(), TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// Get stock info list
        /// </summary>
        /// <param name="begin">begin id</param>
        /// <param name="end">end id</param>
        [HttpGet("GetAll/{begin}/{end}")]
        public void GetAll(int begin, int end)
        {
            BackgroundJob.Schedule<StcokInfoGrabSchedule>(x => x.GrabAll(begin, end), TimeSpan.FromSeconds(3));
        }
    }
}