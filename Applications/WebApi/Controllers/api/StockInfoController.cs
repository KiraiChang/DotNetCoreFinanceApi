using System;
using System.Collections.Generic;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApi.Models;
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
        public ApiResult<IList<StockInfo>> Get()
        {
            var result = _service.GetList();
            if (!result.IsSuccess)
            {
                _logger.LogError(result.InnerException, result.ErrorMessage);
            }

            return new ApiResult<IList<StockInfo>>(result);
        }

        /// <summary>
        /// Get stock info list
        /// </summary>
        /// <returns>list of stock info</returns>
        [HttpGet("Grab")]
        public void Grab()
        {
            BackgroundJob.Schedule<StockInfoGrabSchedule>(x => x.GrabInfo(), TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// Get stock info list
        /// </summary>
        /// <param name="stockId">begin id</param>
        [HttpGet("GetAll/{stockId}")]
        public void GetAll(string stockId)
        {
            BackgroundJob.Schedule<StockInfoGrabSchedule>(x => x.GrabAll(stockId), TimeSpan.FromSeconds(3));
        }
    }
}