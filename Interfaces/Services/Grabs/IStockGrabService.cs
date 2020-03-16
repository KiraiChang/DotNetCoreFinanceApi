using System.Collections.Generic;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;

namespace FinanceApi.Interfaces.Services.Grabs
{
    /// <summary>
    /// interface of stock grab service
    /// </summary>
    public interface IStockGrabService
    {
        /// <summary>
        /// grab list of stock according filter
        /// </summary>
        /// <param name="filter">grab stock filer</param>
        /// <returns>list of stock</returns>
        ServiceResult<IList<Stock>> GetList(StockFilter filter);
    }
}