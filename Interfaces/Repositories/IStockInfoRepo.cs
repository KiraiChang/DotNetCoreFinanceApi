using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Interfaces.Repositories
{
    /// <summary>
    /// Stock relate repository
    /// </summary>
    public interface IStockInfoRepo
    {
        /// <summary>
        /// Get StockInfo list
        /// </summary>
        /// <param name="filter">filter param</param>
        /// <returns>list of stock</returns>
        Task<IList<StockInfo>> GetList(StockInfoFilter filter);

        /// <summary>
        /// Insert StockInfo to db
        /// </summary>
        /// <param name="values">list of stock</param>
        /// <returns>effect count</returns>
        Task<int> Insert(IList<StockInfo> values);
    }
}