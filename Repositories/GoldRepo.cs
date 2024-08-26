using FinanceApi.Interfaces.Repositories;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Repositories.Base;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace FinanceApi.Repositories
{
    /// <summary>
    /// Implement IGoldRepo
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Repositories.IGoldRepo" />
    public class GoldRepo : BaseRepo<Gold>, IGoldRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GoldRepo" /> class.
        /// </summary>
        /// <param name="setting">db connection setting</param>
        public GoldRepo(IOptionsMonitor<ConnectionSetting> setting) : base(setting)
        {
        }

        /// <inheritdoc/>
        public IList<Gold> GetList(GoldFilter filter)
        {
            return base.GetList<GoldFilter>(filter);
        }
    }
}