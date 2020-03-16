using System.Collections.Generic;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;

namespace FinanceApi.Interfaces.Repositories
{
    /// <summary>
    /// Exchange relate service
    /// </summary>
    public interface IExchangeRepo
    {
        /// <summary>
        /// Get Exchange list
        /// </summary>
        /// <param name="filter">filter condition</param>
        /// <returns>service result of exchange list</returns>
        IList<Exchange> GetList(ExchangeFilter filter);

        /// <summary>
        /// Insert exchange into db
        /// </summary>
        /// <param name="values">list of exchange</param>
        /// <returns>service result of effect count</returns>
        int Insert(IList<Exchange> values);
    }
}