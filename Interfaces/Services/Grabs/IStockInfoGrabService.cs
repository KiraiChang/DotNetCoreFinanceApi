using System;
using System.Collections.Generic;
using System.Text;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;

namespace FinanceApi.Interfaces.Services.Grabs
{
    /// <summary>
    /// Interface of StockInfoGrabService
    /// </summary>
    public interface IStockInfoGrabService
    {
        /// <summary>
        /// grab list of StockInfo info
        /// </summary>
        /// <returns>list of stock StockInfo</returns>
        ServiceResult<IList<StockInfo>> GetList();
    }
}