using System;
using System.Collections.Generic;
using FinanceApi.Interfaces.Repositories;
using FinanceApi.Interfaces.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;
using FinanceApi.Models.Services;

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
        /// Initializes a new instance of the <see cref="StockInfoService" /> class.
        /// </summary>
        /// <param name="repo">stock repository</param>
        public StockInfoService(IStockInfoRepo repo)
        {
            _repo = repo;
        }

        /// <inheritdoc cref="IStockInfoService.GetList"/>
        public ServiceResult<IList<StockInfo>> GetList()
        {
            var result = new ServiceResult<IList<StockInfo>>();
            try
            {
                result.InnerResult = _repo.GetList();
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