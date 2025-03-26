using FinanceApi.Interfaces.Repositories;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Repositories.Base;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Repositories
{
    /// <summary>
    /// Implement IStockInfoRepo
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Repositories.IStockInfoRepo" />
    public class StockInfoRepo : BaseRepo<StockInfo>, IStockInfoRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoRepo" /> class.
        /// </summary>
        /// <param name="setting">db connection setting</param>
        public StockInfoRepo(IOptionsMonitor<ConnectionSetting> setting) : base(setting)
        {
        }

        /// <inheritdoc cref="IStockInfoRepo.GetList"/>
        public async Task<IList<StockInfo>> GetList()
        {
            return await base.GetList<StockFilter>(null);
        }
    }
}