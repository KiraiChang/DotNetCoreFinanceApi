using System.Collections.Generic;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;

namespace Finance.Interfaces.Services.Grabs
{
    /// <summary>
    /// interface of Exchange grab service
    /// </summary>
    public interface IExchangeGrabService
    {
        /// <summary>
        /// grab list of exchange according filter
        /// </summary>
        /// <param name="filter">grab exchange filer</param>
        /// <returns>list of exchange</returns>
        ServiceResult<IList<Exchange>> GetList(ExchangeFilter filter);
    }
}