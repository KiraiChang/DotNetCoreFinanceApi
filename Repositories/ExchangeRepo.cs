using System.Collections.Generic;
using Dapper;
using FinanceApi.Interfaces.Repositories;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Repositories.Base;
using FinanceApi.Repositories.TypeHandlers;
using Microsoft.Extensions.Options;

namespace FinanceApi.Repositories
{
    /// <summary>
    /// Exchange Repository
    /// </summary>
    public class ExchangeRepo : BaseRepo<Exchange>, IExchangeRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeRepo" /> class.
        /// </summary>
        /// <param name="setting">db connection setting</param>
        public ExchangeRepo(IOptionsMonitor<ConnectionSetting> setting) : base(setting)
        {
        }

        /// <inheritdoc cref="IExchangeRepo.GetList"/>
        public IList<Exchange> GetList(ExchangeFilter filter)
        {
            return base.GetList<ExchangeFilter>(filter);
        }

        /// <inheritdoc cref="BaseRepo{T}.AddTypeHandler"/>
        protected override void AddTypeHandler()
        {
            SqlMapper.ResetTypeHandlers();
            SqlMapper.AddTypeHandler(new ObjectTypeHandler<IList<ExchangeItem>>());
        }
    }
}