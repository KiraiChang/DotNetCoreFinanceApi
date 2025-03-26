using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Interfaces.Repositories
{
    /// <summary>
    /// gold info repo
    /// </summary>
    public interface IGoldRepo
    {
        /// <summary>
        /// Get gold list
        /// </summary>
        /// <returns>list of stock</returns>
        Task<IList<Gold>> GetList(GoldFilter filter);

        /// <summary>
        /// Insert gold to db
        /// </summary>
        /// <param name="values">list of gold</param>
        /// <returns>effect count</returns>
        Task<int> Insert(IList<Gold> values);
    }
}