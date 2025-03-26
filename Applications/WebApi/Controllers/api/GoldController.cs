using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Schedules;

namespace FinanceApi.Controllers.Api
{
    /// <summary>
    /// Gold Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class GoldController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<GoldController> _logger;

        /// <summary>
        /// Service
        /// </summary>
        private readonly IGoldService _service = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoldController" /> class.
        /// </summary>
        /// <param name="logger">Gold controller logger</param>
        /// <param name="service">Gold service</param>
        public GoldController(ILogger<GoldController> logger, IGoldService service)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get Gold list
        /// </summary>
        /// <param name="beginDate">filter begin date</param>
        /// <param name="endDate">filter end date</param>
        /// <returns>list of Gold</returns>
        [HttpGet]
        public async Task<ApiResult<IList<Gold>>> Get(DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue)
            {
                beginDate = DateTime.Now.Date.AddDays(-1);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }

            var result = await _service.GetList(new GoldFilter()
            {
                BeginDate = beginDate?.Date,
                EndDate = endDate?.Date
            });
            if (!result.IsSuccess)
            {
                _logger.LogError(result.InnerException, result.ErrorMessage);
            }

            return new ApiResult<IList<Gold>>(result);
        }



        /// <summary>
        /// grab special date gold rate
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

            BackgroundJob.Schedule<GoldGrabSchedule>(x => x.Grab(begin.Value, end.Value), TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// grab all date gold rate
        /// </summary>
        [HttpGet("GetAll")]
        public void GrabAll()
        {
            BackgroundJob.Schedule<GoldGrabSchedule>(x => x.GrabAll(), TimeSpan.FromSeconds(3));
        }
    }
}