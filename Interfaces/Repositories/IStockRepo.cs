using System.Collections.Generic;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;

namespace Finance.Interfaces.Repositories
{
    /// <summary>
    /// Stock relate repository
    /// </summary>
    public interface IStockRepo
    {
        /// <summary>
        /// Get stock list
        /// </summary>
        /// <param name="filter">filter condition</param>
        /// <returns>list of stock</returns>
        IList<Stock> GetList(StockFilter filter);

        /// <summary>
        /// Insert stock to db
        /// </summary>
        /// <param name="values">list of stock</param>
        /// <returns>effect count</returns>
        int Insert(IList<Stock> values);
    }
}