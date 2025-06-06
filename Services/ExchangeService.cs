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
    public class ExchangeService : IExchangeService
    {
        /// <summary>
        /// Stock Repository
        /// </summary>
        private IExchangeRepo _repo = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeService" /> class.
        /// </summary>
        /// <param name="repo">exchange repository</param>
        public ExchangeService(IExchangeRepo repo)
        {
            _repo = repo;
        }

        /// <inheritdoc cref="IExchangeService.GetList"/>
        public async Task<ServiceResult<IList<Exchange>>> GetList(ExchangeFilter filter)
        {
            var result = new ServiceResult<IList<Exchange>>();
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

        /// <inheritdoc cref="IExchangeService.Insert(IList{Exchange})"/>
        public async Task<ServiceResult<int>> Insert(IList<Exchange> values)
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