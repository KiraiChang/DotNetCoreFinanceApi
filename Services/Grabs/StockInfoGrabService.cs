using System;
using System.Collections.Generic;
using System.Text;
using FinanceApi.Interfaces.Services.Grabs;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;
using Microsoft.Extensions.Logging;

namespace FinanceApi.Services.Grabs
{
    /// <summary>
    /// stock info grab service
    /// </summary>
    public class StockInfoGrabService : IStockInfoGrabService
    {
        /// <summary>
        /// Logger for ExchangeGrabService
        /// </summary>
        private ILogger<StockInfoGrabService> _logger = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoGrabService" /> class.
        /// </summary>
        /// <param name="logger">logger of StockInfoGrabService</param>
        public StockInfoGrabService(ILogger<StockInfoGrabService> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc cref="IStockInfoGrabService.GetList"/>
        public ServiceResult<IList<StockInfo>> GetList()
        {
            throw new NotImplementedException();
        }
    }
}