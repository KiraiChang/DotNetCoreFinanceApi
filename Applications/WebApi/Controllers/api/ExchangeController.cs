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
        /// <param name="date">filter date</param>
        /// <returns>list of stock</returns>
        [HttpGet]
        public ApiResult<IList<Exchange>> Get(DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Now.Date;
            }

            var result = _service.GetList(new ExchangeFilter()
            {
                Date = date?.Date
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
        /// <param name="date">grab date</param>
        [HttpGet("Grab/{date}")]
        public void Grab(DateTime date)
        {
            BackgroundJob.Schedule<ExchangeGrabSchedule>(x => x.Grab(date), TimeSpan.FromSeconds(3));
        }
    }
}