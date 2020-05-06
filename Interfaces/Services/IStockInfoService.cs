using System;
using System.Collections.Generic;
using System.Text;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;

namespace FinanceApi.Interfaces.Services
{
    /// <summary>
    /// Interface of StockInfoService
    /// </summary>
    public interface IStockInfoService
    {
        /// <summary>
        /// get list of StockInfo info
        /// </summary>
        /// <returns>list of StockInfo info</returns>
        ServiceResult<IList<StockInfo>> GetList();

        /// <summary>
        /// Insert StockInfo into db
        /// </summary>
        /// <param name="values">list of StockInfo</param>
        /// <returns>service result of effect count</returns>
        ServiceResult<int> Insert(IList<StockInfo> values);
    }
}