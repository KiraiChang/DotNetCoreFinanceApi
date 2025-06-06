﻿using FinanceApi.Interfaces.Repositories;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Services
{
    /// <summary>
    /// Implement IStockService
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Services.IStockService" />
    public class StockService : IStockService
    {
        /// <summary>
        /// Stock Repository
        /// </summary>
        private IStockRepo _repo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockService" /> class.
        /// </summary>
        /// <param name="repo">stock repository</param>
        public StockService(IStockRepo repo)
        {
            _repo = repo;
        }

        /// <inheritdoc cref="IStockService.GetList"/>
        public async Task<ServiceResult<IList<Stock>>> GetList(StockFilter filter)
        {
            var result = new ServiceResult<IList<Stock>>();
            try
            {
                result.InnerResult = await _repo.GetList(filter);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.InnerException = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <inheritdoc cref="IStockService.Insert(IList{Stock})"/>
        public async Task<ServiceResult<int>> Insert(IList<Stock> values)
        {
            var result = new ServiceResult<int>();
            try
            {
                result.InnerResult = await _repo.Insert(values);
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.InnerException = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }
    }
}