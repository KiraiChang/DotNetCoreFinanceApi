using System;
using System.Collections.Generic;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

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
        public ServiceResult<IList<Exchange>> Get(DateTime? date)
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

            return new ServiceResult<IList<Exchange>>()
            {
                ErrorCode = result.ErrorCode,
                ErrorMessage = result.ErrorMessage,
                InnerResult = result.InnerResult,
                IsSuccess = result.IsSuccess
            };
        }
    }
}