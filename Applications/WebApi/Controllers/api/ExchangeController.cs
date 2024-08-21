using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WebApi.Models;
using WebApi.Schedules;

namespace FinanceApi.Controllers.Api
{
    /// <summary>
    /// Stock Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ExchangeController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<ExchangeController> _logger;

        /// <summary>
        /// Service
        /// </summary>
        private readonly IExchangeService _service = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeController" /> class.
        /// </summary>
        /// <param name="logger">exchange controller logger</param>
        /// <param name="service">exchange service</param>
        public ExchangeController(ILogger<ExchangeController> logger, IExchangeService service)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get stock list
        /// </summary>
        /// <param name="begin">filter begin date</param>
        /// <param name="end">filter end date</param>
        /// <returns>list of stock</returns>
        [HttpGet]
        public ApiResult<IList<Exchange>> Get(DateTime? begin, DateTime? end)
        {
            if (!end.HasValue)
            {
                end = DateTime.Now.Date;
            }

            if (!begin.HasValue)
            {
                begin = end.Value.AddDays(-14).Date;
            }

            var result = _service.GetList(new ExchangeFilter()
            {
                BeginDate = begin?.Date,
                EndDate = end?.Date
            });
            if (!result.IsSuccess)
            {
                _logger.LogError(result.InnerException, result.ErrorMessage);
            }

            return new ApiResult<IList<Exchange>>(result);
        }

        /// <summary>
        /// grab special date exchange rate
        /// </summary>
        /// <param name="begin">filter begin date</param>
        /// <param name="end">filter end date</param>
        [HttpGet("Grab/{begin}/{end}")]
        public void Grab(DateTime? begin, DateTime? end)
        {
            if (!end.HasValue)
            {
                end = DateTime.Now.Date;
            }

            if (!begin.HasValue)
            {
                begin = end.Value.AddDays(-14).Date;
            }

            BackgroundJob.Schedule<ExchangeGrabSchedule>(x => x.Grab(begin.Value, end.Value), TimeSpan.FromSeconds(3));
        }
    }
}