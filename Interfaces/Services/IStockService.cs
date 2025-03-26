using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Interfaces.Services
{
    /// <summary>
    /// Stock relate service
    /// </summary>
    public interface IStockService
    {
        /// <summary>
        /// Get stock list
        /// </summary>
        /// <param name="filter">filter condition</param>
        /// <returns>service result of stock list</returns>
        Task<ServiceResult<IList<Stock>>> GetList(StockFilter filter);

        /// <summary>
        /// Insert into db
        /// </summary>
        /// <param name="values">list of stock</param>
        /// <returns>service result of effect count</returns>
        Task<ServiceResult<int>> Insert(IList<Stock> values);
    }
}