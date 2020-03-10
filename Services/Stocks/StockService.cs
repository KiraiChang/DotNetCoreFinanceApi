using System;
using System.Collections.Generic;
using System.Text;
using Finance.Interfaces.Repositories;
using Finance.Interfaces.Services;
using FinanceApi.Models.Services;
using FinanceApi.Models.Entity;
using FinanceApi.Models.Filter;

namespace FinanceApi.Services.Stocks
{
    /// <summary>
    /// Implement IStockService
    /// </summary>
    /// <seealso cref="Finance.Interfaces.Services.IStockService" />
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
        public ServiceResult<IList<Stock>> GetList(StockFilter filter)
        {
            var result = new ServiceResult<IList<Stock>>();
            try
            {
                result.InnerResult = _repo.GetList(filter);
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
        public ServiceResult<int> Insert(IList<Stock> values)
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