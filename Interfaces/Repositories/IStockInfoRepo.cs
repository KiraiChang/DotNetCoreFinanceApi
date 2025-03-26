using FinanceApi.Models.Entity;
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
        /// <returns>list of stock</returns>
        Task<IList<StockInfo>> GetList();

        /// <summary>
        /// Insert StockInfo to db
        /// </summary>
        /// <param name="values">list of stock</param>
        /// <returns>effect count</returns>
        Task<int> Insert(IList<StockInfo> values);
    }
}