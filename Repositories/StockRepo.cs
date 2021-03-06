﻿using System.Collections.Generic;
using FinanceApi.Interfaces.Repositories;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Repositories.Base;
using Microsoft.Extensions.Options;

namespace FinanceApi.Repositories
{
    /// <summary>
    /// Implement IStockRepo
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Repositories.IStockRepo" />
    public class StockRepo : BaseRepo<Stock>, IStockRepo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StockRepo" /> class.
        /// </summary>
        /// <param name="setting">db connection setting</param>
        public StockRepo(IOptionsMonitor<ConnectionSetting> setting) : base(setting)
        {
        }

        /// <inheritdoc cref="IStockRepo.GetList"/>
        public IList<Stock> GetList(StockFilter filter)
        {
            return base.GetList<StockFilter>(filter);
        }
    }
}