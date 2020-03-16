using System.Collections.Generic;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;

namespace FinanceApi.Interfaces.Services
{
    /// <summary>
    /// Exchange relate service
    /// </summary>
    public interface IExchangeService
    {
        /// <summary>
        /// Get Exchange list
        /// </summary>
        /// <param name="filter">filter condition</param>
        /// <returns>service result of exchange list</returns>
        ServiceResult<IList<Exchange>> GetList(ExchangeFilter filter);

        /// <summary>
        /// Insert exchange into db
        /// </summary>
        /// <param name="values">list of exchange</param>
        /// <returns>service result of effect count</returns>
        ServiceResult<int> Insert(IList<Exchange> values);
    }
}