using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace FinanceApi.Controllers.Api
{
    /// <summary>
    /// Stock Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        /// <summary>
        /// Logger
        /// </summary>
        private readonly ILogger<StockController> _logger;

        /// <summary>
        /// Service
        /// </summary>
        private readonly IStockService _service = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockController" /> class.
        /// </summary>
        /// <param name="logger">stock controller logger</param>
        /// <param name="service">stock service</param>
        public StockController(ILogger<StockController> logger, IStockService service)
        {
            _service = service;
            _logger = logger;
        }

        /// <summary>
        /// Get stock list
        /// </summary>
        /// <param name="stockId">stock id</param>
        /// <param name="beginDate">filter begin date</param>
        /// <param name="endDate">filter end date</param>
        /// <returns>list of stock</returns>
        [HttpGet]
        public async Task<ApiResult<IList<Stock>>> Get(string stockId, DateTime? beginDate, DateTime? endDate)
        {
            if (!beginDate.HasValue)
            {
                beginDate = DateTime.Now.Date.AddDays(-1);
            }

            if (!endDate.HasValue)
            {
                endDate = DateTime.Now.Date;
            }

            var result = await _service.GetList(new StockFilter()
            {
                StockId = stockId,
                BeginDate = beginDate?.Date,
                EndDate = endDate?.Date
            });
            if (!result.IsSuccess)
            {
                _logger.LogError(result.InnerException, result.ErrorMessage);
            }

            return new ApiResult<IList<Stock>>(result);
        }
    }
}