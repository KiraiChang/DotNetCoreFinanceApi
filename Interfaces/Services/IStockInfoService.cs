using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        Task<ServiceResult<IList<StockInfo>>> GetList();

        /// <summary>
        /// Insert StockInfo into db
        /// </summary>
        /// <param name="values">list of StockInfo</param>
        /// <returns>service result of effect count</returns>
        Task<ServiceResult<int>> Insert(IList<StockInfo> values);
    }
}