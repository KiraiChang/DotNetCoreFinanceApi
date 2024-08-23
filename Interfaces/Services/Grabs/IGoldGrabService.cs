using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using System.Collections.Generic;

namespace FinanceApi.Interfaces.Services.Grabs
{
    /// <summary>
    /// gold grab service
    /// </summary>
    public interface IGoldGrabService
    {
        /// <summary>
        /// Grab gold
        /// </summary>
        /// <param name="filter">gold filter</param>
        /// <returns>gold result</returns>
        ServiceResult<IList<Gold>> GetList(GoldFilter filter);
    }
}