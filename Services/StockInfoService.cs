using FinanceApi.Interfaces.Repositories;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Services;
using System;
using System.Collections.Generic;

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
        private IStockInfoRepo _repo = null;

        /// <summary>
        /// Cache Service
        /// </summary>
        private ICacheService _cacheService = null;

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
        public ServiceResult<IList<StockInfo>> GetList()
        {
            var result = new ServiceResult<IList<StockInfo>>();
            try
            {
                result.InnerResult = _cacheService.GetOrAddAsync(nameof(GetList), nameof(GetList), async () =>
                {
                    return _repo.GetList();
                }, DateTime.Now.AddHours(1)).ConfigureAwait(false).GetAwaiter().GetResult();
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
        public ServiceResult<int> Insert(IList<StockInfo> values)
        {
            var result = new ServiceResult<int>();
            try
            {
                result.InnerResult = _repo.Insert(values);
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