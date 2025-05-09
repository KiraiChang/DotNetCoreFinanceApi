﻿using FinanceApi.Interfaces.Repositories;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinanceApi.Services
{
    /// <summary>
    /// Implement IStockInfoService
    /// </summary>
    /// <seealso cref="FinanceApi.Interfaces.Services.IStockInfoService" />
    public class StockInfoService : IStockInfoService
    {
        /// <summary>
        /// Stock Repository
        /// </summary>
        private readonly IStockInfoRepo _repo = null;

        /// <summary>
        /// Cache Service
        /// </summary>
        private readonly ICacheService _cacheService = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="StockInfoService" /> class.
        /// </summary>
        /// <param name="repo">stock repository</param>
        /// <param name="cacheService">cache service</param>
        public StockInfoService(IStockInfoRepo repo,
            ICacheService cacheService)
        {
            _repo = repo;
            _cacheService = cacheService;
        }

        /// <inheritdoc cref="IStockInfoService.GetList"/>
        public async Task<ServiceResult<IList<StockInfo>>> GetList()
        {
            var result = new ServiceResult<IList<StockInfo>>();
            try
            {
                result.InnerResult = await _cacheService.GetOrAddAsync(nameof(GetList), nameof(GetList), async () =>
                {
                    return await _repo.GetList();
                }, DateTime.Now.AddHours(1));
                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                result.InnerException = ex;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <inheritdoc cref="IStockInfoService.Insert(IList{StockInfo})"/>
        public async Task<ServiceResult<int>> Insert(IList<StockInfo> values)
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